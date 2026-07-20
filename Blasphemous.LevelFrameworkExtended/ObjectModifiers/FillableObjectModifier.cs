using Blasphemous.Framework.Levels;
using Blasphemous.LevelFrameworkExtended.ObjectModifiers.Properties;
using Blasphemous.ModdingAPI;
using Tools.Level.Layout;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

public abstract class FillableObjectModifier
{
    private enum FillAxis { X, Y }

    protected Vector2 _size;
    protected Vector2 _offset;

    /// <summary>
    /// No-op by default. Override to add custom behavior for single-object placement.
    /// </summary>
    protected virtual void ApplySingleObject(GameObject obj, ObjectData data)
    {
    }

    protected void ApplyFillObjects(GameObject obj, ObjectData data, FillableProperties props)
    {
        if (props.XBoundary == FillableProperties.BoundaryType.None
            && props.YBoundary == FillableProperties.BoundaryType.None)
        {
            ModLog.Error($"At least one boundary should be not `none`! " +
                $"Skipped registering object!");
            UnityEngine.Object.Destroy(obj);
        }
        else if (props.XBoundary == FillableProperties.BoundaryType.None
            && props.YBoundary != FillableProperties.BoundaryType.None)
        {
            ApplyFillObjectsOnAxis(obj, data, props, FillAxis.Y);
        }
        else if (props.XBoundary != FillableProperties.BoundaryType.None
            && props.YBoundary == FillableProperties.BoundaryType.None)
        {
            ApplyFillObjectsOnAxis(obj, data, props, FillAxis.X);
        }
        else
        {
            ModLog.Error($"{data.id} x and y boundaries both specified, " +
                $"this feature is currently unsupported, " +
                $"skipped registering object!");
            UnityEngine.Object.Destroy(obj);
        }
    }

    /// <summary>
    /// Returns the GameObject to clone when filling. Override to clone a child instead.
    /// </summary>
    protected virtual GameObject GetSourceForFill(GameObject obj)
    {
        return obj;
    }

    /// <summary>
    /// Called after all fill segments are created. 
    /// Default destroys the original object.
    /// Override to keep the original alive or add post-processing.
    /// </summary>
    protected virtual void OnAfterFill(GameObject obj, ObjectData data,
        float from, float to, int numSegments, float segmentSize)
    {
        UnityEngine.Object.Destroy(obj);
    }

    private void ApplyFillObjectsOnAxis(GameObject obj, ObjectData data, FillableProperties props, FillAxis axis)
    {
        // extract axis-specific boundary values
        float? fromRaw, toRaw;
        FillableProperties.BoundaryType boundary;

        if (axis == FillAxis.Y)
        {
            fromRaw = props.Bottom;
            toRaw = props.Top;
            boundary = props.YBoundary;
        }
        else
        {
            fromRaw = props.Left;
            toRaw = props.Right;
            boundary = props.XBoundary;
        }

        // validate boundary values
        string dimLabel = axis == FillAxis.Y ? "top/bottom" : "left/right";
        if (fromRaw == null)
        {
            ModLog.Error($"{data.id} {dimLabel} not specified!\n" +
                $"Skipped registering object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }
        if (toRaw == null)
        {
            ModLog.Error($"{data.id} {dimLabel} not specified!\n" +
                $"Skipped registering object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        float from = fromRaw.Value;
        float to = toRaw.Value;
        float segSize = axis == FillAxis.Y ? _size.y : _size.x;
        float range = to - from;
        bool isYAxis = axis == FillAxis.Y;

        // calculate integer number of segments based on boundary rounding
        int numSegments;
        if (boundary == FillableProperties.BoundaryType.Inner)
        {
            numSegments = (int)Mathf.Floor(range / segSize);
        }
        else if (boundary == FillableProperties.BoundaryType.Outer)
        {
            numSegments = (int)Mathf.Ceil(range / segSize);
        }
        else if (boundary == FillableProperties.BoundaryType.Exact)
        {
            float floatNumSegments = range / segSize;
            if (Mathf.Abs(floatNumSegments - Mathf.RoundToInt(floatNumSegments)) > 1e-3)
            {
                ModLog.Error($"{data.id} has boundary condition `exact` " +
                $"but boundaries given are not exact!\n" +
                $"Skipped registering object!");
                UnityEngine.Object.Destroy(obj);
                return;
            }
            numSegments = Mathf.RoundToInt(floatNumSegments);
        }
        else
        {
            ModLog.Error($"Invalid boundary type for {data.id}!\n" +
                $"Skipped registering object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        if (numSegments <= 0)
        {
            ModLog.Error($"{data.id} specified boundaries are shorter than 1 segment!\n" +
                $"Skipped registering object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        // create GameObjects for each segment
        bool adjustFromBottomOrLeft = isYAxis
            ? (props.YAdjust == FillableProperties.YAdjustDirection.Bottom)
            : (props.XAdjust == FillableProperties.XAdjustDirection.Left);
        GameObject source = GetSourceForFill(obj);
        Vector3 basePos = obj.transform.position;

        for (int i = 0; i < numSegments; i++)
        {
            GameObject newObject = UnityEngine.Object.Instantiate(
                source,
                UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);

            float segmentPos = adjustFromBottomOrLeft
                ? from + ((i + 0.5f) * segSize)
                : to - ((i + 0.5f) * segSize);

            Vector3 newPos = basePos;
            if (axis == FillAxis.Y)
                newPos.y = segmentPos;
            else
                newPos.x = segmentPos;

            newObject.transform.position = newPos;
            newObject.name = $"{data.id}_Segment_{i}";
        }

        OnAfterFill(obj, data, from, to, numSegments, segSize);
    }
}
