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
            var iSpeed2 = projectileSpeed * projectileSpeed;
            var tSpeed2 = aTargetSpeed.sqrMagnitude;
            var fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
            var targetDist2 = targetDir.sqrMagnitude;
            var d = fDot1 * fDot1 - targetDist2 * (tSpeed2 - iSpeed2);

            if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
                return Vector3.zero;

            var sqrt = Mathf.Sqrt(d);
            var s1 = (-fDot1 - sqrt) / targetDist2;
            var s2 = (-fDot1 + sqrt) / targetDist2;
            if (s1 < 0.0001f)
            {
                if (s2 < 0.0001f)
                    return Vector3.zero;
                return s2 * targetDir + aTargetSpeed;
            }
            if (s2 < 0.0001f)
                return s1 * targetDir + aTargetSpeed;
            if (s1 < s2)
                return s2 * targetDir + aTargetSpeed;
            return s1 * targetDir + aTargetSpeed;
        }
    }
}