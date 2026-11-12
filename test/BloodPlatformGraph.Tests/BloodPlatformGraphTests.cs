using Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;
using System;
using System.Linq;
using Xunit;

namespace Blasphemous.LevelFrameworkExtended.BloodPlatformGraphTests;

public class BloodPlatformGraphTests
{
    [Fact]
    public void ParseDefaults()
    {
        BloodPlatformGraph graph = Parse(
            "node=a",
            "node=b",
            "initial=a",
            "edge=a --> b");

        BloodPlatformNode a = graph.Nodes.First(node => node.Id == "a");
        Assert.Equal("a", a.Id);
        Assert.Equal(Vector3Data.Zero, a.Position);
        Assert.Equal(Vector3Data.Zero, a.Rotation);
        Assert.Equal(Vector3Data.One, a.Scale);
        Assert.Equal("b", Assert.Single(a.Targets));
        Assert.True(a.IsInitial);
    }

    [Fact]
    public void ParseExplicitTransformsAndWhitespace()
    {
        BloodPlatformGraph graph = Parse(
            "  node = a ; position = [ 1, 2.5, -3 ] ; rotation = [ 0, 90, 180 ] ; scale = [ 2, 0.5, 1 ] ",
            "initial = a");

        BloodPlatformNode a = graph.Nodes.First(node => node.Id == "a");
        Assert.Equal(new Vector3Data(1f, 2.5f, -3f), a.Position);
        Assert.Equal(new Vector3Data(0f, 90f, 180f), a.Rotation);
        Assert.Equal(new Vector3Data(2f, 0.5f, 1f), a.Scale);
        Assert.True(a.IsInitial);
    }

    [Fact]
    public void ParseMultipleInitialsAndDirectedLinks()
    {
        BloodPlatformGraph graph = Parse(
            "node=a",
            "node=b",
            "node=c",
            "initial=a",
            "initial=b",
            "edge=a --> b",
            "edge=b --> c");

        BloodPlatformNode a = graph.Nodes.First(node => node.Id == "a");
        BloodPlatformNode b = graph.Nodes.First(node => node.Id == "b");
        BloodPlatformNode c = graph.Nodes.First(node => node.Id == "c");
        Assert.Equal(2, graph.Nodes.Count(node => node.IsInitial));
        Assert.True(a.IsInitial);
        Assert.True(b.IsInitial);
        Assert.False(c.IsInitial);
        Assert.Equal("b", Assert.Single(a.Targets));
        Assert.Equal("c", Assert.Single(b.Targets));
        Assert.Empty(c.Targets);
    }

    [Fact]
    public void ParseBranchingConvergenceAndCycle()
    {
        BloodPlatformGraph graph = Parse(
            "node=a",
            "node=b",
            "node=c",
            "node=d",
            "initial=a",
            "edge=a --> b",
            "edge=a --> c",
            "edge=b --> d",
            "edge=c --> d",
            "edge=d --> a");

        BloodPlatformNode a = graph.Nodes.First(node => node.Id == "a");
        BloodPlatformNode b = graph.Nodes.First(node => node.Id == "b");
        BloodPlatformNode c = graph.Nodes.First(node => node.Id == "c");
        BloodPlatformNode d = graph.Nodes.First(node => node.Id == "d");
        Assert.Equal(2, a.Targets.Count);
        Assert.Equal("d", Assert.Single(b.Targets));
        Assert.Equal("d", Assert.Single(c.Targets));
        Assert.Equal("a", Assert.Single(d.Targets));
    }

    [Fact]
    public void ParseEmptyBatchLabel()
    {
        BloodPlatformGraph graph = BloodPlatformGraph.Parse(new[] { "node=a", "initial=a" }, "");
        Assert.Equal("<unnamed>", graph.Batch);
    }

    [Theory]
    [InlineData("node=a; position=[1,2,3", "node 'a'", "malformed")]
    [InlineData("node=a; position", "node 'a'", "malformed")]
    [InlineData("node=", "node declaration", "node id")]
    [InlineData("initial=", "initial declaration", "id")]
    [InlineData("edge=a -->", "edge declaration", "to")]
    [InlineData("edge= --> b", "edge declaration", "from")]
    [InlineData("node=a; node=b", "node 'a'", "duplicate 'node'")]
    [InlineData("node=a; initial=a", "node 'a'", "unknown field 'initial'")]
    public void RejectMalformedDeclaration(string declaration, string context, string detail)
    {
        AssertGraphError(new[] { declaration }, context, detail);
    }

    [Fact]
    public void RejectDuplicateNode()
    {
        AssertGraphError(new[] { "node=a", "node=a", "initial=a" }, "node 'a'", "duplicate node");
    }

    [Fact]
    public void AllowRepeatedInitial()
    {
        BloodPlatformGraph graph = Parse("node=a", "initial=a", "initial=a");
        Assert.True(Assert.Single(graph.Nodes).IsInitial);
    }

    [Fact]
    public void RejectSelfLink()
    {
        AssertGraphError(new[] { "node=a", "initial=a", "edge=a --> a" }, "edge 'a --> a'", "self-link");
    }

    [Fact]
    public void RejectDuplicateEdge()
    {
        AssertGraphError(
            new[] { "node=a", "node=b", "initial=a", "edge=a --> b", "edge=a --> b" },
            "edge 'a --> b'",
            "duplicate edge");
    }

    [Fact]
    public void RejectUndefinedInitial()
    {
        AssertGraphError(new[] { "node=a", "initial=b" }, "initial 'b'", "undefined node");
    }

    [Fact]
    public void RejectUndefinedEdgeEndpoint()
    {
        AssertGraphError(new[] { "node=a", "initial=a", "edge=a --> b" }, "edge 'a --> b'", "undefined node 'b'");
        AssertGraphError(new[] { "node=a", "initial=a", "edge=b --> a" }, "edge 'b --> a'", "undefined node 'b'");
    }

    [Fact]
    public void RejectMissingNodes()
    {
        AssertGraphError(new[] { "initial=a" }, "no node declarations");
    }

    [Fact]
    public void RejectMissingInitial()
    {
        AssertGraphError(new[] { "node=a" }, "no initial declarations");
    }

    [Fact]
    public void RejectUnreachableNode()
    {
        AssertGraphError(
            new[] { "node=a", "node=b", "node=c", "initial=a", "edge=a --> b" },
            "graph batch",
            "unreachable node 'c'");
    }

    [Fact]
    public void RejectUndirectedArrow()
    {
        AssertGraphError(
            new[] { "node=a", "node=b", "initial=a", "edge=a -> b" },
            "edge declaration",
            "'-->'");
    }

    [Fact]
    public void RejectUnknownField()
    {
        AssertGraphError(new[] { "node=a; color=red" }, "node 'a'", "unknown field 'color'");
        AssertGraphError(new[] { "position=[1,2,3]" }, "declaration", "unknown declaration type");
    }

    [Theory]
    [InlineData("node=a; position=[1,2]", "position", "three")]
    [InlineData("node=a; scale=[1,2,three]", "scale", "number")]
    [InlineData("node=a; rotation=[1,2,3]garbage", "rotation", "malformed")]
    public void RejectVectorWithWrongArityOrNonNumber(string declaration, string field, string detail)
    {
        AssertGraphError(new[] { declaration }, "node 'a'", field, detail);
    }

    [Theory]
    [InlineData("NaN")]
    [InlineData("Infinity")]
    [InlineData("-Infinity")]
    public void RejectNonFiniteVector(string value)
    {
        AssertGraphError(new[] { $"node=a; position=[{value},0,0]" }, "node 'a'", "position", "non-finite");
    }

    [Fact]
    public void RejectDuplicateVectorField()
    {
        AssertGraphError(
            new[] { "node=a; position=[1,2,3]; position=[4,5,6]" },
            "node 'a'",
            "duplicate 'position'");
    }

    [Fact]
    public void RejectEmptyElement()
    {
        AssertGraphError(new[] { "node=a", "" }, "declaration", "empty");
        AssertGraphError(new[] { "node=a; position=[]" }, "node 'a'", "position", "three");
    }

    [Fact]
    public void RejectTrailingSemicolon()
    {
        AssertGraphError(new[] { "node=a;" }, "malformed");
    }

    private static BloodPlatformGraph Parse(params string[] declarations) =>
        BloodPlatformGraph.Parse(declarations, "test-batch");

    private static void AssertGraphError(string[] declarations, params string[] fragments)
    {
        BloodPlatformGraphException ex = Assert.Throws<BloodPlatformGraphException>(
            () => BloodPlatformGraph.Parse(declarations, "graph batch"));
        foreach (string fragment in fragments)
        {
            Assert.True(
                ex.Message.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0,
                $"Expected '{fragment}' in: {ex.Message}");
        }
    }
}
