using UnityEngine;

namespace Extras
{
public static class Extensions
{
    /// <summary>
    /// Converts a Vector2 into a Vector3. Transformation is XY -> XZ.
    /// </summary>
    /// <param name="vector2">Value to be converted to Vector3.</param>
    /// <param name="y">Y component of the transformed vector.</param>
    /// <returns></returns>
    public static Vector3 ToVector3(this Vector2 vector2, float y = 0f)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }

    /// <summary>
    /// Flips the variable and returns new state.
    /// </summary>
    /// <param name="b">Value to be toggled.</param>
    public static bool Toggle(this ref bool b)
    {
        b = !b;
        return b;
    }

    /// <summary>
    /// Returns 1 if true, 0 if false.
    /// </summary>
    /// <param name="b">Value to cast as integer.</param>
    /// <returns></returns>
    public static int AsInt(this bool b)
    {
        return b ? 1 : 0;
    }

    /// <summary>
    /// Sets a new value to a single axis of a Vector3.
    /// </summary>
    /// <param name="vector3">Vector whose axis value needs to be set.</param>
    /// <param name="axis">Pick the axis whose value is to be changed. Must be 'x', 'y', or 'z'.</param>
    /// <param name="newValue">Value to set to the axis.</param>
    /// <returns>Vector with the new value.</returns>
    /// <exception cref="System.ArgumentException">Thrown when axis is not valid.</exception>
    public static Vector3 SetAxis(this ref Vector3 vector3, char axis, float newValue)
    {
        switch (axis)
        {
            case 'x':
                vector3.Set(newValue, vector3.y, vector3.z);
                break;
            case 'y':
                vector3.Set(vector3.x, newValue, vector3.z);
                break;
            case 'z':
                vector3.Set(vector3.x, vector3.y, newValue);
                break;
            default:
                throw new System.ArgumentException($"Axis {axis} does not exist. Expected 'x', 'y', or 'z'.");
        }

        return vector3;
    }
}
}