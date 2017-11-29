using UnityEngine;

namespace Assets.Mechanics
{
    public static class TargetingMechanis
    {
        /// <summary>
        /// Calculates aim velocity vector from shooter position to moving target, counts with target speed
        /// </summary>
        /// <param name="enemy">Enemy transform</param>
        /// <param name="shooterPosition">Shooter position, the projectile muzzle</param>
        /// <param name="projectileSpeed">Projectile speed</param>
        /// <returns></returns>
        public static Vector3 CalculateAimVelocityVector(Transform enemy, Vector3 shooterPosition, float projectileSpeed)
        {
            var targetVelocity = new Vector3(0, 0, 0);
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

        /// <summary>
        /// Locks on target in certain direction
        /// </summary>
        /// <param name="direction">Direction to aim</param>
        /// <param name="center">Aiming position</param>
        /// <param name="rotation">Aiming rotation</param>
        /// <returns></returns>
        public static GameObject LockTarget(Vector3 direction, Vector3 center, Quaternion rotation, float maxDistance)
        {
            RaycastHit info;
            var dir = rotation * direction;
            if (Physics.Raycast(center, dir, out info))
            {       
                var target = info.transform.gameObject;

                // check distance
                var distance = (target.transform.position - center).magnitude;
                if (distance > maxDistance)
                    return null;

                // target is damagable
                if (target.GetComponent<IDamagable>() != null)
                {
                    return target;
                }
            }
            return null;
        }

        /// <summary>
        /// Calculate aim and rotation for projectile 
        /// </summary>
        /// <param name="targetPosition">Position of the target</param>
        /// <param name="shooterPosition">Position of the shooter</param>
        /// <returns></returns>
        public static Quaternion CalculateAimRotation(Vector3 targetPosition, Vector3 shooterPosition)
        {
            // Aim at the targetPosition
            // TargetPosition substracting towerPosition creates a vector pointing from the tower to the targetPosition. 
            var aimPoint = new Vector3(targetPosition.x, 0, targetPosition.z) - shooterPosition;
            return Quaternion.LookRotation(aimPoint);
        }
    }
}