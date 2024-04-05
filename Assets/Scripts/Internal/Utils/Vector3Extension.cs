
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 RotateAround(this Vector3 position, Vector3 axis, float angle)
    {
        Vector3 point = Quaternion.AngleAxis(angle, axis) * position;
        Vector3 resultVec3 = point;
        return resultVec3;
    }
}