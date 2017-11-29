using Assets.Mechanics;
using Assets.Scripts.Constants;
using Assets.Scripts.Powerups.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class MissilePowerup : ProjectilePowerupBase
    {
        public MissilePowerup()
        {
            Projectile = new MissilePowerupProjectile();
        }

        /// <summary>
        /// Take object in front and lock it as a target
        /// </summary>
        /// <param name="center">Shooter position</param>
        /// <param name="rotation">Rotation of the shooter</param>
        /// <returns>Locked target</returns>
        public override GameObject LockTarget(Vector3 center, Quaternion rotation)
        {            
            var direction = rotation * Vector3.forward;
            var target = TargetingMechanis.LockTarget(direction, center, Range, NumberConstants.DetetionAngle);
            // new target detected
            if (target != null)
            {
                Target = target;        
            }

            // verify old or new target
            if (Target != null)
            {
                // target not in range
                if (!TargetingMechanis.IsTargetInRange(Target, direction, center, Range, NumberConstants.DetetionAngle))
                {
                    Target = null;
                }
                else
                {
                    // TODO draw rectangle arround locked target
                    Debug.DrawRay(center, Target.transform.position - center, Color.green);
                }
            }
            return Target;
        }
    }

    public class ReverseMissilePowerup : ProjectilePowerupBase
    {
        public ReverseMissilePowerup()
        {
            Projectile = new ReverseMissilePowerupProjectile();
        }

        public override GameObject LockTarget(Vector3 center, Quaternion rotation)
        {
            var direction = rotation * Vector3.back;
            var target = TargetingMechanis.LockTarget(direction, center, Range, NumberConstants.DetetionAngle);
            // new target detected
            if (target != null)
            {
                Target = target;
            }

            // verify old or new target
            if (Target != null)
            {
                // target not in range
                if (!TargetingMechanis.IsTargetInRange(Target, direction, center, Range, NumberConstants.DetetionAngle))
                {
                    Target = null;
                }
                else
                {
                    // TODO draw rectangle arround locked target
                    Debug.DrawRay(center, Target.transform.position - center, Color.green);
                }
            }
            return Target;
        }
    }
}