using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class ReverseMissilePowerup : ProjectilePowerupBase
    {
        protected override GameObject GetProjectilePrefab()
        {
            return Resources.Load<GameObject>("reverse_missile_projectile");
        }

        public override Sprite GetPowerupIcon()
        {
            var sprite = new Sprite();
            throw new System.NotImplementedException();
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