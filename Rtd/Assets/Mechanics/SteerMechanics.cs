using UnityEngine;

public static class SteerMechanics
{
    /// <summary>
    /// Steers vector using given angle
    /// </summary>
    /// <param name="vector">Vector to be steered</param>
    /// <param name="angle">Angle of steering</param>
    /// <returns>New instance of vector</returns>
    public static Quaternion Steer(Vector3 vector, float angle)
    {
        var quat = new Quaternion(vector.x, vector.y, vector.z, 1);
        return Quaternion.Euler(0, angle, 0) * quat;        
    }
}