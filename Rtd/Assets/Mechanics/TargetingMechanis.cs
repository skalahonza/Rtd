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
            return CalculateAimVelocityVector(enemy.rotation, enemy.position, targetVelocity, shooterPosition, projectileSpeed);
        }

        public static Vector3 CalculateAimVelocityVector(Transform enemy, Vector3 targetVelocity,
            Vector3 shooterPosition, float projectileSpeed)
        {
            return CalculateAimVelocityVector(enemy.rotation, enemy.position, targetVelocity, shooterPosition, projectileSpeed);
        }

        private static Vector3 CalculateAimVelocityVector(Quaternion targetRot, Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 projectilePosition, float projectileSpeed)
        {
            var targetDir = aTargetPos - projectilePosition;
            var iSpeed2 = projectileSpeed * projectileSpeed;
            var tSpeed2 = aTargetSpeed.sqrMagnitude;
            var fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
            var targetDist2 = targetDir.sqrMagnitude;
            var d = fDot1 * fDot1 - targetDist2 * (tSpeed2 - iSpeed2);

            if (d < 0.1f) // negative == no possible course because the interceptor isn't fast enough
            {
                //emulate speed
                var newSpeed = targetRot * Vector3.forward;
                //newSpeed *= 100 / 3.6f;
                newSpeed *= aTargetSpeed.magnitude / 2;
                return CalculateAimVelocityVector(targetRot, aTargetPos, newSpeed, projectilePosition, projectileSpeed);
            }

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
        /// Locks on target in certain direction, target must be IDamagable
        /// </summary>
        /// <param name="direction">Direction of aming, typically rotation*Vector3.forward</param>
        /// <param name="center">Aiming position</param>
        /// <param name="maxDistance">Max distance for adminig</param>
        /// <param name="maxAngle">Max angle for aiming</param>
        /// <returns>Null if no damagable target found in given direction, otherwise return the gameobject to shoot on.</returns>
        public static GameObject LockTarget(Vector3 direction, Vector3 center, float maxDistance, float maxAngle)
        {
            RaycastHit info;
            if (Physics.Raycast(center, direction, out info))
            {
                var target = info.transform.gameObject;

                if (!IsTargetInRange(target, direction, center, maxDistance, maxAngle))
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
        /// Verify if the target is in range for shooting
        /// </summary>
        /// <param name="target">Target I want to shoot</param>
        /// <param name="direction">Direction of aim</param>
        /// <param name="position">Shooter's position</param>
        /// <param name="maxDistance">Max distance for shooting</param>
        /// <param name="maxAngle">Max angle for detection</param>
        /// <returns>True if I can lock on target</returns>
        public static bool IsTargetInRange(GameObject target, Vector3 direction, Vector3 position, float maxDistance, float maxAngle)
        {
            // check distance and angle
            var distance = (target.transform.position - position).magnitude;
            var angle = Vector3.Angle(direction, target.transform.position - position);

            return !(distance > maxDistance) && !(angle > maxAngle);
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