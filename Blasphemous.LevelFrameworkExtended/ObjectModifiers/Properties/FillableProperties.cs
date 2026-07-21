using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;

/// <summary>
/// Typed properties parsed from ObjectData.properties for fillable objects
/// </summary>
public class FillableProperties
{
    /// <summary>Whether to place a single object or fill an area</summary>
    public enum ObjectMethod
    {
        /// <summary>Places a single object</summary>
        Single,
        /// <summary>Repeatedly places objects to fill an area</summary>
        Fill
    }

    /// <summary>How to resolve boundary rounding</summary>
    public enum BoundaryType
    {
        /// <summary>No boundary constraint</summary>
        None,
        /// <summary>Rounds boundary inward (floor)</summary>
        Inner,
        /// <summary>Rounds boundary outward (ceil)</summary>
        Outer,
        /// <summary>Boundary must match exactly</summary>
        Exact
    }

    /// <summary>Adjustment direction along X axis</summary>
    public enum XAdjustDirection
    {
        /// <summary>Adjusts from left</summary>
        Left,
        /// <summary>Adjusts from right</summary>
        Right
    }

    /// <summary>Adjustment direction along Y axis</summary>
    public enum YAdjustDirection
    {
        /// <summary>Adjusts from top</summary>
        Top,
        /// <summary>Adjusts from bottom</summary>
        Bottom
    }

    /// <summary>Single or fill placement</summary>
    [JsonProperty("object_method")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ObjectMethod Method { get; set; } = ObjectMethod.Single;

    /// <summary>Boundary condition along X axis</summary>
    [JsonProperty("x_boundary")]
    [JsonConverter(typeof(StringEnumConverter))]
    public BoundaryType XBoundary { get; set; } = BoundaryType.None;

    /// <summary>Boundary condition along Y axis</summary>
    [JsonProperty("y_boundary")]
    [JsonConverter(typeof(StringEnumConverter))]
    public BoundaryType YBoundary { get; set; } = BoundaryType.None;

    /// <summary>Adjustment direction along X axis</summary>
    [JsonProperty("x_adjust")]
    [JsonConverter(typeof(StringEnumConverter))]
    public XAdjustDirection XAdjust { get; set; } = XAdjustDirection.Left;

    /// <summary>Adjustment direction along Y axis</summary>
    [JsonProperty("y_adjust")]
    [JsonConverter(typeof(StringEnumConverter))]
    public YAdjustDirection YAdjust { get; set; } = YAdjustDirection.Top;

    /// <summary>Top boundary position</summary>
    [JsonProperty("top")]
    public float? Top { get; set; }

    /// <summary>Bottom boundary position</summary>
    [JsonProperty("bottom")]
    public float? Bottom { get; set; }

    /// <summary>Left boundary position</summary>
    [JsonProperty("left")]
    public float? Left { get; set; }

    /// <summary>Right boundary position</summary>
    [JsonProperty("right")]
    public float? Right { get; set; }

    /// <summary>
    /// Parses a string[] property array into typed FillableProperties using Newtonsoft.Json
    /// </summary>
    public static FillableProperties Parse(string[] raw)
    {
        JObject jObj = [];
        foreach (string arg in raw)
        {
            int sep = arg.IndexOf('=');
            string key = arg.Substring(0, sep).Trim().ToLower();
            string value = arg.Substring(sep + 1).Trim().ToLower();
            jObj[key] = value;
        }
        return jObj.ToObject<FillableProperties>();
    }
}
