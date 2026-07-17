using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.ModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;
/// <summary>
/// Modifier for solid objects that cannot be passed by TPO
/// </summary>
public class ColliderModifer : FillableObjectModifier, IModifier
{
    /// <summary>
    /// The layer of the collider. 
    /// For solid collider, choose layer "Floor". 
    /// For droppable platforms, choose layer "OneWayDown"
    /// </summary>
    private string _layer;

    /// <summary>
    /// Specify the size of the collider (offset set to zero)
    /// </summary>
    public ColliderModifer(string layer, Vector2 size)
        : this(layer, size, Vector2.zero) { }

    /// <summary>
    /// Specify the size and offset of the collider
    /// </summary>
    public ColliderModifer(string layer, Vector2 size, Vector2 offset)
    {
        _size = size;
        _offset = offset;
        _layer = layer;
    }

    /// <summary>
    /// Adds a collider component and sets the layer to `Floor`
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {
        _validPropertyArguments = new()
        {
            { "object_method", x => new List<string> {"single", "fill"}.Contains(x) },
            { "x_boundary", x => new List<string> {"none", "inner", "outer", "exact"}.Contains(x) },
            { "y_boundary", x => new List<string> {"none", "inner", "outer", "exact"}.Contains(x) },
            { "x_adjust", x => new List<string> {"left", "right"}.Contains(x) },
            { "y_adjust", x => new List<string> {"top", "bottom"}.Contains(x) },
            { "top", x => Main.floatRegex.IsMatch(x) },
            { "bottom", x => Main.floatRegex.IsMatch(x) },
            { "left", x => Main.floatRegex.IsMatch(x) },
            { "right", x => Main.floatRegex.IsMatch(x) },
        };
        _defaultPropertyArguments = new()
        {
            { "object_method", "single" },
            { "x_boundary", "none" },
            { "y_boundary", "none" },
            { "x_adjust", "left" },
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

        obj.name = $"{data.id}";
        obj.layer = LayerMask.NameToLayer(_layer);
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;

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
}

