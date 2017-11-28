using Assets.Mechanics;
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
            var target = TargetingMechanis.LockTarget(Vector3.forward, center, rotation, Range);
            if (target != null)
            {
                Target = target;
            }
            return target;
        }
    }
}