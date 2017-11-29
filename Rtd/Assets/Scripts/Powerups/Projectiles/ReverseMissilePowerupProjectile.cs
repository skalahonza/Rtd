using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    public class ReverseMissilePowerupProjectile : ProjectileBase {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("reverse_missile_projectile");
        }
    }
}