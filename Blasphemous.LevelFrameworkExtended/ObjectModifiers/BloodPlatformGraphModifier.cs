using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;
using Blasphemous.ModdingAPI;
using System.Collections.Generic;
using System.Linq;
using Tools.Level.Actionables;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

/// <summary>
/// Creates one ordinary FaithPlatform per graph node and wires its directed links.
/// </summary>
public class BloodPlatformGraphModifier : IModifier
{
    /// <inheritdoc/>
    public void Apply(GameObject obj, ObjectData data)
    {
        BloodPlatformGraph graph;
        try
        {
            graph = BloodPlatformGraph.Parse(data.properties, data.id);
        }
        catch (BloodPlatformGraphException ex)
        {
            ModLog.Error(ex.Message);
            Object.Destroy(obj);
            return;
        }

        Transform holder = obj.transform.parent;
        if (holder == null)
        {
            ModLog.Error($"platform-blood-graph '{graph.Batch}': graph addition has no scene holder");
            Object.Destroy(obj);
            return;
        }

        GameObject template = BloodPlatformGraphTemplate.Current;
        if (template == null || template.GetComponent<FaithPlatform>() == null)
        {
            ModLog.Error($"platform-blood-graph '{graph.Batch}': ordinary platform template is missing FaithPlatform");
            Object.Destroy(obj);
            return;
        }

        Dictionary<string, GameObject> objects = graph.Nodes.ToDictionary(
            node => node.Id,
            node => CreateNode(node, template, holder));
        foreach (BloodPlatformNode node in graph.Nodes)
        {
            ApplyLinks(objects[node.Id], node, objects);
        }

        Object.Destroy(obj);
    }

    private static GameObject CreateNode(BloodPlatformNode node, GameObject template, Transform holder)
    {
        GameObject platform = Object.Instantiate(template, holder);
        platform.name = node.Id;
        platform.transform.position = new Vector3(node.Position.X, node.Position.Y, node.Position.Z);
        platform.transform.eulerAngles = new Vector3(node.Rotation.X, node.Rotation.Y, node.Rotation.Z);
        platform.transform.localScale = new Vector3(node.Scale.X, node.Scale.Y, node.Scale.Z);
        platform.SetActive(true);
        return platform;
    }

    private static void ApplyLinks(
        GameObject platform,
        BloodPlatformNode node,
        Dictionary<string, GameObject> objects)
    {
        BloodPlatformGraphLink.Current = new BloodPlatformGraphLink(
            node.IsInitial,
            node.Targets.Select(target => objects[target]).ToArray());
        try
        {
            platform.GetComponent<FaithPlatform>().Use();
        }
        finally
        {
            BloodPlatformGraphLink.Current = null;
        }
    }
}

internal sealed class BloodPlatformGraphLink
{
    internal static BloodPlatformGraphLink Current { get; set; }

    internal bool IsInitial { get; }
    internal GameObject[] Targets { get; }

    internal BloodPlatformGraphLink(bool isInitial, GameObject[] targets)
    {
        IsInitial = isInitial;
        Targets = targets;
    }
}
