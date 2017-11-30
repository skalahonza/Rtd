using Assets.Mechanics;
using Assets.Scripts.Constants;
using Assets.Scripts.Powerups.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
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
            NewTrgetFound(target, direction, center);
            return Target;
        }
    }
}