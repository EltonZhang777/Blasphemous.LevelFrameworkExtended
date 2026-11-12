using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;

/// <summary>
/// Parsed and fully validated declaration of one blood-platform route.
/// </summary>
public sealed class BloodPlatformGraph
{
    /// <summary>Nodes in declaration order.</summary>
    public List<BloodPlatformNode> Nodes { get; }
    /// <summary>Graph batch label used in diagnostics.</summary>
    public string Batch { get; }

    private BloodPlatformGraph(string batch, List<BloodPlatformNode> nodes)
    {
        Batch = string.IsNullOrEmpty(batch) ? "<unnamed>" : batch;
        Nodes = nodes;
    }

    /// <summary>
    /// Parses and validates a complete graph. Throws before returning if any declaration
    /// or graph invariant is invalid.
    /// </summary>
    public static BloodPlatformGraph Parse(IEnumerable<string> declarations, string batch)
    {
        List<BloodPlatformNode> nodes = [];
        List<string> initials = [];
        List<GraphEdge> edges = [];
        int index = 0;

        foreach (string declaration in declarations ?? new string[0])
        {
            ParseDeclaration(declaration, index++, batch, nodes, initials, edges);
        }

        Validate(nodes, initials, edges, batch);
        return new BloodPlatformGraph(batch, nodes);
    }

    private static void ParseDeclaration(
        string declaration,
        int index,
        string batch,
        List<BloodPlatformNode> nodes,
        List<string> initials,
        List<GraphEdge> edges)
    {
        string context = $"properties[{index}]";
        if (string.IsNullOrEmpty(declaration) || declaration.Trim().Length == 0)
            throw Error(batch, context, "declaration is empty");

        int equals = declaration.IndexOf('=');
        if (equals < 0)
            throw Error(batch, context, $"malformed declaration '{declaration.Trim()}': missing '='");

        string kind = declaration.Substring(0, equals).Trim();
        string value = declaration.Substring(equals + 1);
        switch (kind)
        {
            case "node":
                nodes.Add(ParseNode(value, context, batch));
                break;
            case "initial":
                initials.Add(ParseId(value, context, batch, "initial declaration"));
                break;
            case "edge":
                edges.Add(ParseEdge(value, context, batch));
                break;
            default:
                throw Error(batch, context, $"unknown declaration type '{kind}'");
        }
    }

    private static BloodPlatformNode ParseNode(string value, string context, string batch)
    {
        if (value.Trim().Length == 0)
            throw Error(batch, context, "node declaration is missing its node ID");
        string id = ParseId(value.Split(';')[0], context, batch, "node declaration");
        context = $"node '{id}' ({context})";
        string[] fields = SplitFields(value, context, batch);

        HashSet<string> seen = new(StringComparer.Ordinal);
        Vector3Data position = Vector3Data.Zero;
        Vector3Data rotation = Vector3Data.Zero;
        Vector3Data scale = Vector3Data.One;

        foreach (string field in fields.Skip(1))
        {
            int equals = field.IndexOf('=');
            if (equals < 0)
                throw Error(batch, context, $"malformed field '{field}': missing '='");

            string key = field.Substring(0, equals).Trim();
            string fieldValue = field.Substring(equals + 1).Trim();
            if (!seen.Add(key))
                throw Error(batch, context, $"duplicate '{key}' field");
            if (key == "node")
                throw Error(batch, context, "duplicate 'node' declaration in one node field list");

            switch (key)
            {
                case "position":
                    position = ParseVector(fieldValue, key, context, batch);
                    break;
                case "rotation":
                    rotation = ParseVector(fieldValue, key, context, batch);
                    break;
                case "scale":
                    scale = ParseVector(fieldValue, key, context, batch);
                    break;
                default:
                    throw Error(batch, context, $"unknown field '{key}'");
            }
        }

        return new BloodPlatformNode(id, position, rotation, scale);
    }

    private static GraphEdge ParseEdge(string value, string context, string batch)
    {
        string[] sides = value.Split(new[] { "-->" }, StringSplitOptions.None);
        if (sides.Length != 2)
            throw Error(batch, context, $"edge declaration '{value.Trim()}' must contain exactly one '-->'");

        string from = ParseEdgeEndpoint(sides[0], "from", context, batch);
        string to = ParseEdgeEndpoint(sides[1], "to", context, batch);
        return new GraphEdge(from, to);
    }

    private static string ParseEdgeEndpoint(string value, string side, string context, string batch)
    {
        string id = value.Trim();
        if (id.Length == 0)
            throw Error(batch, context, $"edge declaration is missing its {side} endpoint");
        return id;
    }

    private static string[] SplitFields(string value, string context, string batch)
    {
        string[] fields = value.Split(';').Select(field => field.Trim()).ToArray();
        if (fields.Any(field => field.Length == 0))
            throw Error(batch, context, "malformed declaration: empty field or trailing ';'");
        return fields;
    }

    private static string ParseId(string value, string context, string batch, string what)
    {
        string id = value.Trim();
        if (id.Length == 0)
            throw Error(batch, context, $"{what} is missing its ID");
        return id;
    }

    private static Vector3Data ParseVector(string value, string field, string context, string batch)
    {
        if (value.Length < 2 || value[0] != '[' || value[value.Length - 1] != ']')
            throw Error(batch, context, $"{field} '{value}' is malformed: expected [x,y,z]");

        string[] parts = value.Substring(1, value.Length - 2).Split(',').Select(part => part.Trim()).ToArray();
        if (parts.Length != 3)
            throw Error(batch, context, $"{field} '{value}' must contain exactly three numbers");

        float[] numbers = new float[3];
        for (int i = 0; i < parts.Length; i++)
        {
            if (!float.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out numbers[i]))
                throw Error(batch, context, $"{field} '{value}' contains a non-number '{parts[i]}'");
            if (float.IsNaN(numbers[i]) || float.IsInfinity(numbers[i]))
                throw Error(batch, context, $"{field} '{value}' contains a non-finite number '{parts[i]}'");
        }

        return new Vector3Data(numbers[0], numbers[1], numbers[2]);
    }

    private static void Validate(
        List<BloodPlatformNode> nodes,
        List<string> initials,
        List<GraphEdge> edges,
        string batch)
    {
        if (nodes.Count == 0)
            throw Error(batch, "graph", "graph has no node declarations");
        if (initials.Count == 0)
            throw Error(batch, "graph", "graph has no initial declarations");

        Dictionary<string, BloodPlatformNode> byId = new(StringComparer.Ordinal);
        foreach (BloodPlatformNode node in nodes)
        {
            if (byId.ContainsKey(node.Id))
                throw Error(batch, $"node '{node.Id}'", "duplicate node ID");
            byId.Add(node.Id, node);
        }

        foreach (string initial in initials)
        {
            if (!byId.ContainsKey(initial))
                throw Error(batch, $"initial '{initial}'", "undefined node");
            BloodPlatformNode node = byId[initial];
            node.IsInitial = true;
        }

        HashSet<string> edgeSeen = new(StringComparer.Ordinal);
        foreach (GraphEdge edge in edges)
        {
            string label = $"edge '{edge.From} --> {edge.To}'";
            if (edge.From == edge.To)
                throw Error(batch, label, "self-link is not allowed");
            if (!edgeSeen.Add(edge.Key))
                throw Error(batch, label, "duplicate edge");
            if (!byId.ContainsKey(edge.From))
                throw Error(batch, label, $"undefined node '{edge.From}'");
            if (!byId.ContainsKey(edge.To))
                throw Error(batch, label, $"undefined node '{edge.To}'");
            byId[edge.From]._targets.Add(edge.To);
        }

        HashSet<string> reachable = new(initials, StringComparer.Ordinal);
        Queue<string> pending = new(initials);
        while (pending.Count > 0)
        {
            foreach (string target in byId[pending.Dequeue()].Targets)
            {
                if (reachable.Add(target))
                    pending.Enqueue(target);
            }
        }

        string[] unreachable = nodes.Where(node => !reachable.Contains(node.Id)).Select(node => node.Id).ToArray();
        if (unreachable.Length > 0)
            throw Error(batch, "graph", $"unreachable node '{unreachable[0]}' (from every initial platform)");
    }

    private static BloodPlatformGraphException Error(string batch, string context, string detail) =>
        new($"platform-blood-graph '{(string.IsNullOrEmpty(batch) ? "<unnamed>" : batch)}' {context}: {detail}");

    private sealed class GraphEdge
    {
        public string From { get; }
        public string To { get; }
        public string Key => $"{From}\u0001{To}";

        public GraphEdge(string from, string to)
        {
            From = from;
            To = to;
        }
    }
}

/// <summary>A declared platform node and its directed outgoing links.</summary>
public sealed class BloodPlatformNode
{
    internal readonly List<string> _targets = [];

    /// <summary>Node ID, also used as the platform instance name.</summary>
    public string Id { get; }
    /// <summary>Absolute node position.</summary>
    public Vector3Data Position { get; }
    /// <summary>Node Euler rotation.</summary>
    public Vector3Data Rotation { get; }
    /// <summary>Node local scale.</summary>
    public Vector3Data Scale { get; }
    /// <summary>Whether this node is an entry platform; assigned during validation.</summary>
    public bool IsInitial { get; internal set; }
    /// <summary>Outgoing target node IDs in edge declaration order.</summary>
    public List<string> Targets => _targets;

    internal BloodPlatformNode(
        string id,
        Vector3Data position,
        Vector3Data rotation,
        Vector3Data scale)
    {
        Id = id;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}

/// <summary>Three floats parsed from a graph vector field.</summary>
public readonly struct Vector3Data
{
    /// <summary>X component.</summary>
    public float X { get; }
    /// <summary>Y component.</summary>
    public float Y { get; }
    /// <summary>Z component.</summary>
    public float Z { get; }

    /// <summary>Creates a vector value.</summary>
    public Vector3Data(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>(0, 0, 0)</summary>
    public static Vector3Data Zero => new(0f, 0f, 0f);
    /// <summary>(1, 1, 1)</summary>
    public static Vector3Data One => new(1f, 1f, 1f);
}

/// <summary>Raised when a blood-platform graph declaration is invalid.</summary>
public sealed class BloodPlatformGraphException : Exception
{
    /// <summary>Creates the exception with a complete diagnostic message.</summary>
    public BloodPlatformGraphException(string message) : base(message) { }
}
