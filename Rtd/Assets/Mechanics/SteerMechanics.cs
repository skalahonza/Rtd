using UnityEngine;

public static class SteerMechanics
{
    public const float MinimumSteeringSpeed = 5f;

    /// <summary>
    /// Steers vector using given angle
    /// </summary>
    /// <returns>New instance of vector</returns>
    public static Vector3 Steer(Vector3 velocity, float wheelTurn, float steerRadius)
    {
        var magnitude = velocity.magnitude;
        return new Vector3(0f, wheelTurn * magnitude / (steerRadius * 1), 0f);
    }
}