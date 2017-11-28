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
        void Start()
        {
            
        }

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