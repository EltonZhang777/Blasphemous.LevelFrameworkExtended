using Blasphemous.Framework.Levels;
using Blasphemous.ModdingAPI;
using Tools.Level.Layout;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

public abstract class FillableObjectModifier : ModifierWithProperties
{
    protected int _numSegmentsX;
    protected int _numSegmentsY;
    protected Vector2 _size;
    protected Vector2 _offset;

    protected void ApplySingleObject(GameObject obj, ObjectData data)
    {
        // since `Apply()` already applied a single object,
        // nothing needs to be done here
    }

    protected void ApplyFillObjects(GameObject obj, ObjectData data)
    {
        // first, determine the boundary conditions
        if (_properties["x_boundary"] == "none" && _properties["y_boundary"] == "none")
        {
            // no boundary specified, abort

            ModLog.Error($"At least one boundary should be not `none`! " +
                $"Skipped registering object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }
        else if (_properties["x_boundary"] == "none" && _properties["y_boundary"] != "none")
        {
            // y_boundary specified, fill a vertical column of objects

            // parse the string of top and bottom to floats
            if (!_properties.TryGetValue("top", out string top_string))
            {
                ModLog.Error($"platform {data.id} top boundary not specified!\n" +
                    $"Skipped registering object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }
            if (!_properties.TryGetValue("bottom", out string bottom_string))
            {
                ModLog.Error($"platform {data.id} top boundary not specified!\n" +
                    $"Skipped registering object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }
            float top = float.Parse(top_string);
            float bottom = float.Parse(bottom_string);

            // calculate the integer number of segments based on tile length
            // round number of segments based on `y_boundary`
            if (_properties["y_boundary"] == "inner")
            {
                _numSegmentsY = (int)Mathf.Floor((top - bottom) / _size.y);
            }
            else if (_properties["y_boundary"] == "outer")
            {
                _numSegmentsY = (int)Mathf.Ceil((top - bottom) / _size.y);
            }
            else if (_properties["y_boundary"] == "exact")
            {
                float floatNumSegments = (top - bottom) / _size.y;
                if (Mathf.Abs(floatNumSegments - Mathf.RoundToInt(floatNumSegments)) > 1e-3)
                {
                    ModLog.Error($"Ladder {data.id} has boundary condition `exact` " +
                    $"but boundaries given are not exact!\n" +
                    $"Skipped registering ladder object!");
                    UnityEngine.Object.Destroy(obj);
                    return;
                }
                _numSegmentsY = Mathf.RoundToInt(floatNumSegments);
            }

            if (_numSegmentsY <= 0)
            {
                ModLog.Error($"Ladder {data.id} top and bottom position " +
                    $"specified is shorter than 1 segment!\n" +
                    $"Skipped registering ladder object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }

            // create `GameObject`s of each segment
            // and adjust each segment's position based on `y_adjust`
            if (_properties["y_adjust"] == "top")
            {
                for (int i = 0; i < _numSegmentsY; i++)
                {
                    GameObject newObject = UnityEngine.Object.Instantiate(
                        obj,
                        UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);
                    newObject.transform.position = new Vector3(
                        obj.transform.position.x,
                        top - ((i + 0.5f) * _size.y),
                        0f);
                    newObject.name = $"{data.id}_Segment_{i}";
                }
            }
            else if (_properties["y_adjust"] == "bottom")
            {
                for (int i = 0; i < _numSegmentsY; i++)
                {
                    GameObject newObject = UnityEngine.Object.Instantiate(
                        obj,
                        UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);
                    newObject.transform.position = new Vector3(
                        obj.transform.position.x,
                        bottom + ((i + 0.5f) * _size.y),
                        0f);
                    newObject.name = $"{data.id}_Segment_{i}";
                }
            }

            UnityEngine.Object.Destroy(obj); // destroy the original object
        }
        else if (_properties["x_boundary"] != "none" && _properties["y_boundary"] == "none")
        {
            // x_boundary specified, fill a horizontal row of objects

            // parse the string of left and right to floats
            if (!_properties.TryGetValue("left", out string left_string))
            {
                ModLog.Error($"platform {data.id} left boundary not specified!\n" +
                    $"Skipped registering object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }
            if (!_properties.TryGetValue("right", out string right_string))
            {
                ModLog.Error($"platform {data.id} top boundary not specified!\n" +
                    $"Skipped registering object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }
            float left = float.Parse(left_string);
            float right = float.Parse(right_string);

            // calculate the integer number of segments based on tile length
            // round number of segments based on `x_boundary`
            if (_properties["x_boundary"] == "inner")
            {
                _numSegmentsX = (int)Mathf.Floor((right - left) / _size.x);
            }
            else if (_properties["x_boundary"] == "outer")
            {
                _numSegmentsX = (int)Mathf.Ceil((right - left) / _size.x);
            }
            else if (_properties["x_boundary"] == "exact")
            {
                float floatNumSegments = (right - left) / _size.x;
                if (Mathf.Abs(floatNumSegments - Mathf.RoundToInt(floatNumSegments)) > 1e-3)
                {
                    ModLog.Error($"Object {data.id} has boundary condition `exact` " +
                    $"but boundaries given are not exact!\n" +
                    $"Skipped registering object!");
                    UnityEngine.Object.Destroy(obj);
                    return;
                }
                _numSegmentsX = Mathf.RoundToInt(floatNumSegments);
            }

            if (_numSegmentsX <= 0)
            {
                ModLog.Error($"{data.id} top and bottom position " +
                    $"specified is shorter than 1 segment!\n" +
                    $"Skipped registering object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }

            // create `GameObject`s of each segment
            // and adjust each segment's position based on `x_adjust`
            if (_properties["x_adjust"] == "right")
            {
                for (int i = 0; i < _numSegmentsX; i++)
                {
                    GameObject newObject = UnityEngine.Object.Instantiate(
                        obj,
                        UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);
                    newObject.transform.position = new Vector3(
                        right - ((i + 0.5f) * _size.x),
                        obj.transform.position.y,
                        0f);
                    newObject.name = $"{data.id}_Segment_{i}";
                }
            }
            else if (_properties["x_adjust"] == "left")
            {
                for (int i = 0; i < _numSegmentsX; i++)
                {
                    GameObject newObject = UnityEngine.Object.Instantiate(
                        obj,
                        UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);
                    newObject.transform.position = new Vector3(
                        left + ((i + 0.5f) * _size.x),
                        obj.transform.position.y,
                        0f);
                    newObject.name = $"{data.id}_Segment_{i}";
                }
            }

            UnityEngine.Object.Destroy(obj); // destroy the original object
        }
        else if (_properties["x_boundary"] != "none" && _properties["y_boundary"] != "none")
        {
            // x_boundary and y_boundary both specified,
            // shouldn't be needing to fill a matrix of object (at least in near future)
            // code WIP, now just throws an error

            ModLog.Error($"{data.id} x and y boundaries both specified, " +
                $"this feature is currently unsupported, " +
                $"skipped registering object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }
    }
}
