using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    public class MissilePowerupProjectile:ProjectileBase
    {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("missile_powerup");
        }
    }
}