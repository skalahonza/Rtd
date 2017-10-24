using UnityEngine;

public static class SteerMechanics
{
    public static Vector3 Steer(Vector3 vector, float angle)
    {
        return Quaternion.Euler(0, -45, 0) * vector;
    }
}