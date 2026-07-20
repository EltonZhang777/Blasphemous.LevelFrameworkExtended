using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;
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
        var props = FillableProperties.Parse(data.properties);

        obj.name = $"{data.id}";
        obj.layer = LayerMask.NameToLayer(_layer);
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;

        switch (props.Method)
        {
            case FillableProperties.ObjectMethod.Single:
                ApplySingleObject(obj, data);
                break;
            case FillableProperties.ObjectMethod.Fill:
                ApplyFillObjects(obj, data, props);
                break;
        }
    }
}
