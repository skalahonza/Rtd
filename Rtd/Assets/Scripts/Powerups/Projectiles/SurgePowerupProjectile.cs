using System;
using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    public class SurgePowerupProjectile : ProjectileBase {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("surge_powerup_projectile");
        }

        public override void OnHit(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}