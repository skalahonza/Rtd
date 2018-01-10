using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    /// <summary>
    /// Apply reverse missile powerup on a car
    /// </summary>
    public class ReverseMissilePowerup : ProjectilePowerupBase
    {
        protected override GameObject GetProjectilePrefab()
        {
            return Resources.Load<GameObject>("reverse_missile_projectile");
        }

        public override Sprite GetPowerupIcon()
        {
            return ImageMechanics.LoadSprite("reverse");
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