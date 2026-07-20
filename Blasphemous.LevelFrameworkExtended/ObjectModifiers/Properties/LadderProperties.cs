using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;

/// <summary>
/// Typed properties parsed from ObjectData.properties for ladder objects
/// </summary>
public class LadderProperties
{
    /// <summary>Whether to place a single ladder or fill an area</summary>
    public enum ObjectMethod { Single, Fill }

    /// <summary>How to resolve boundary rounding (no None — ladders always fill)</summary>
    public enum BoundaryType { Inner, Outer, Exact }

    /// <summary>Single or fill placement</summary>
    [JsonProperty("object_method")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ObjectMethod Method { get; set; } = ObjectMethod.Single;

    /// <summary>Boundary condition along Y axis</summary>
    [JsonProperty("y_boundary")]
    [JsonConverter(typeof(StringEnumConverter))]
    public BoundaryType YBoundary { get; set; } = BoundaryType.Inner;

    /// <summary>Adjustment direction along Y axis</summary>
    [JsonProperty("y_adjust")]
    [JsonConverter(typeof(StringEnumConverter))]
    public FillableProperties.YAdjustDirection YAdjust { get; set; } = FillableProperties.YAdjustDirection.Top;

    /// <summary>Top boundary position (required for fill)</summary>
    [JsonProperty("top")]
    public float? Top { get; set; }

    /// <summary>Bottom boundary position (required for fill)</summary>
    [JsonProperty("bottom")]
    public float? Bottom { get; set; }

    /// <summary>
    /// Parses a string[] property array into typed LadderProperties using Newtonsoft.Json
    /// </summary>
    public static LadderProperties Parse(string[] raw)
    {
        var jObj = new JObject();
        foreach (string arg in raw)
        {
            int sep = arg.IndexOf('=');
            string key = arg.Substring(0, sep).Trim().ToLower();
            string value = arg.Substring(sep + 1).Trim().ToLower();
            jObj[key] = value;
        }
        return jObj.ToObject<LadderProperties>();
    }
}
