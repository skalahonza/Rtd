using System;
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
            // new target detected
            var target = TargetingMechanis.LockTarget(Vector3.forward, center, rotation, Range);
            if (target != null)
            {
                Target = target;
                Debug.DrawRay(center, Target.transform.position - center, Color.blue);                
            }

            if (Target != null)
            {
                // check target radius and distance
                var direction = rotation * Vector3.forward;
                var angle = Vector3.Angle(direction, Target.transform.position - center);
                var distance = (Target.transform.position - center).magnitude;

                Debug.Log(angle);

                // remove targets out of reach
                if (distance > Range || angle > NumberConstants.DetetionAngle)
                {
                    Target = null;
                }
                else
                {
                    // TODO draw rectangle arround locked target
                    Debug.DrawRay(center, Target.transform.position - center, Color.green);
                    //Debug.DrawRay(center,TargetingMechanis.CalculateAimVelocityVector(Target.transform, center, Projectile.Speed), Color.red);
                }
            }


            return Target;
        }
    }
}