using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

/// <summary>
/// Modifier for ladders
/// </summary>
public class LadderModifier : FillableObjectModifier, IModifier
{
    private float _segmentLength;

    /// <summary>
    /// Destroys original SpriteRender, 
    /// then creates a series of SpriteRenderers for each segment of the ladder, 
    /// before creating the one actual hitbox
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {
        LadderProperties props = LadderProperties.Parse(data.properties);

        obj.name = $"LadderTrigger_{data.id}";
        obj.layer = LayerMask.NameToLayer("Ladder");

        switch (props.Method)
        {
            case LadderProperties.ObjectMethod.Single:
                ApplySingleObject(obj, data);
                break;
            case LadderProperties.ObjectMethod.Fill:
                base.ApplyFillObjects(obj, data, ConvertToFillableProperties(props));
                break;
        }
    }

    /// <summary>
    /// Adds a BoxCollider2D trigger for single-segment ladders
    /// </summary>
    protected override void ApplySingleObject(GameObject obj, ObjectData data)
    {
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(0.5f, _segmentLength);
    }

    /// <summary>
    /// Clones the SpriteRenderer's GameObject instead of the root object
    /// </summary>
    protected override GameObject GetSourceForFill(GameObject obj)
    {
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        return sprite != null ? sprite.gameObject : obj;
    }

    /// <summary>
    /// Destroys the original sprite and creates a single covering collider
    /// </summary>
    protected override void OnAfterFill(GameObject obj, ObjectData data,
        float from, float to, int numSegments, float segmentSize)
    {
        // destroy the original sprite renderer
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        if (sprite != null)
            UnityEngine.Object.Destroy(sprite);

        // position the collider at the center of the entire ladder
        obj.transform.position = new Vector3(
            obj.transform.position.x,
            (to + from) / 2,
            0f);

        // create one single hitbox covering all segments
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(0.5f, segmentSize * numSegments);
    }

    /// <summary>
    /// Construct a ladder with custom single segment y-length
    /// </summary>
    public LadderModifier(float segmentLength)
    {
        _segmentLength = segmentLength;
        _size = new Vector2(segmentLength, segmentLength);
    }

    private static FillableProperties ConvertToFillableProperties(LadderProperties ladder)
    {
        FillableProperties.BoundaryType fb;
        switch (ladder.YBoundary)
        {
            case LadderProperties.BoundaryType.Inner: fb = FillableProperties.BoundaryType.Inner; break;
            case LadderProperties.BoundaryType.Outer: fb = FillableProperties.BoundaryType.Outer; break;
            case LadderProperties.BoundaryType.Exact: fb = FillableProperties.BoundaryType.Exact; break;
            default: fb = FillableProperties.BoundaryType.Inner; break;
        }

        return new FillableProperties
        {
            Method = ladder.Method == LadderProperties.ObjectMethod.Single
                ? FillableProperties.ObjectMethod.Single
                : FillableProperties.ObjectMethod.Fill,
            YBoundary = fb,
            YAdjust = ladder.YAdjust,
            Top = ladder.Top,
            Bottom = ladder.Bottom,
        };
    }
}
