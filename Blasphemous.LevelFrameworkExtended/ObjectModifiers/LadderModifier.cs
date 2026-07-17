using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.ModdingAPI;
using System.Collections.Generic;
using Tools.Level.Layout;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

/// <summary>
/// Modifier for ladders
/// </summary>
public class LadderModifier : ModifierWithProperties, IModifier
{
    private float _segmentLength;
    private int _numSegments;

    /// <summary>
    /// Destroys original SpriteRender, 
    /// then creates a series of SpriteRenderers for each segment of the ladder, 
    /// before creating the one actual hitbox
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {

        _validPropertyArguments = new()
        {
            { "object_method", x => new List<string> {"fill", "single"}.Contains(x) },
            { "y_boundary", x => new List<string> {"inner", "outer", "exact"}.Contains(x) },
            { "y_adjust", x => new List<string> {"top", "bottom"}.Contains(x) }
        };
        _defaultPropertyArguments = new()
        {
            { "y_boundary", "inner" },
            { "y_adjust", "top" }
        };

        _properties = UnzipProperties(data.properties);
        if (_properties == null)
        {
            ModLog.Error($"Invalid properties specifications for {data.id}, " +
                $"skipped registering object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        obj.name = $"LadderTrigger_{data.id}";
        obj.layer = LayerMask.NameToLayer("Ladder");

        switch (_properties["object_method"])
        {
            case "single":
                ApplySingleObject(obj, data);
                break;
            case "fill":
                ApplyFillObjects(obj, data);
                break;
        }
    }

    private void ApplySingleObject(GameObject obj, ObjectData data)
    {
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(
            0.5f,
            _segmentLength);
    }

    private void ApplyFillObjects(GameObject obj, ObjectData data)
    {
        // parse the string of top and bottom to floats
        if (!_properties.TryGetValue("top", out string top_string))
        {
            ModLog.Error($"Ladder {data.id} top position not specified!\n" +
                $"Skipped registering ladder object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }
        if (!_properties.TryGetValue("bottom", out string bottom_string))
        {
            ModLog.Error($"Ladder {data.id} bottom position not specified!\n" +
                $"Skipped registering ladder object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }
        float top = float.Parse(top_string);
        float bottom = float.Parse(bottom_string);

        // calculate the integer number of ladder segments based on tile length
        // calculate number of segments based on `y_boundary`
        if (_properties["y_boundary"] == "inner")
        {
            _numSegments = (int)Mathf.Floor((top - bottom) / _segmentLength);
        }
        else if (_properties["y_boundary"] == "outer")
        {
            _numSegments = (int)Mathf.Ceil((top - bottom) / _segmentLength);
        }
        else if (_properties["y_boundary"] == "exact")
        {
            float floatNumSegments = (top - bottom) / _segmentLength;
            if (Mathf.Abs(floatNumSegments - Mathf.RoundToInt(floatNumSegments)) > 1e-3)
            {
                ModLog.Error($"Ladder {data.id} has boundary condition `exact` " +
                $"but boundaries given are not exact!\n" +
                $"Skipped registering ladder object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }
            _numSegments = Mathf.RoundToInt(floatNumSegments);
        }

        if (_numSegments <= 0)
        {
            ModLog.Error($"Ladder {data.id} top and bottom position " +
                $"specified is shorter than 1 segment!\n" +
                $"Skipped registering ladder object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        // create `GameObject`s of each segment
        // and adjust each segment's position based on `y_adjust`
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        if (_properties["y_adjust"] == "top")
        {
            for (int i = 0; i < _numSegments; i++)
            {
                GameObject newObject = UnityEngine.Object.Instantiate(
                    sprite.gameObject,
                    UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);
                newObject.transform.position = new Vector3(
                    obj.transform.position.x,
                    top - ((i + 0.5f) * _segmentLength),
                    0f);
                newObject.name = $"{data.id}_LadderSegment_{i}";
                newObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        else if (_properties["y_adjust"] == "bottom")
        {
            for (int i = 0; i < _numSegments; i++)
            {
                GameObject newObject = UnityEngine.Object.Instantiate(
                    sprite.gameObject,
                    UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);
                newObject.transform.position = new Vector3(
                    obj.transform.position.x,
                    bottom + ((i + 0.5f) * _segmentLength),
                    0f);
                newObject.name = $"{data.id}_LadderSegment_{i}";
                newObject.layer = LayerMask.NameToLayer("Default");
            }
        }

        UnityEngine.Object.Destroy(sprite); // destroy the original sprite

        // create one single collider for the ladder
        obj.transform.position = new Vector3(
            obj.transform.position.x,
            (top + bottom) / 2,
            0f);
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(
            0.5f,
            _segmentLength * _numSegments);
    }

    /// <summary>
    /// Construct a ladder with custom single segment y-length
    /// </summary>
    public LadderModifier(float segmentLength)
    {
        _segmentLength = segmentLength;
    }
}

