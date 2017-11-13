using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Mechanics
{
    public static class TargetingMechanis
    {
        public static Vector3 CalculateAimVelocityVector(Transform enemy, Vector3 shooterPosition, float projectileSpeed)
        {
            var targetVelocity = new Vector3(0,0,0);
            var rb = enemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                targetVelocity = rb.velocity;
            }
            return CalculateAimVelocityVector(enemy.position, targetVelocity, shooterPosition, projectileSpeed);
        }

        private static Vector3 CalculateAimVelocityVector(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 projectilePosition, float projectileSpeed)
        {
            var targetDir = aTargetPos - projectilePosition;
            float iSpeed2 = projectileSpeed * projectileSpeed;
            float tSpeed2 = aTargetSpeed.sqrMagnitude;
            float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
            float targetDist2 = targetDir.sqrMagnitude;
            float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);

            if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
                return Vector3.zero;

            float sqrt = Mathf.Sqrt(d);
            float S1 = (-fDot1 - sqrt) / targetDist2;
            float S2 = (-fDot1 + sqrt) / targetDist2;
            if (S1 < 0.0001f)
            {
                if (S2 < 0.0001f)
                    return Vector3.zero;
                return (S2) * targetDir + aTargetSpeed;
            }
            if (S2 < 0.0001f)
                return (S1) * targetDir + aTargetSpeed;
            if (S1 < S2)
                return (S2) * targetDir + aTargetSpeed;
            return (S1) * targetDir + aTargetSpeed;
        }
    }
}