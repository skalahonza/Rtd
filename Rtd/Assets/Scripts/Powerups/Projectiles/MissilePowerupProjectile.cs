﻿using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    public class MissilePowerupProjectile:ProjectileBase
    {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("missile_projectile");
        }

        public override void OnHit(Collider other)
        {
            var parent = other.gameObject;
            if (parent.Equals(Owner)) return;
            IDamagable target;
            if ((target = parent.GetComponent<IDamagable>()) != null)
            {
                target.SufferDamage(this);
            }
            base.OnHit(other);
        }
    }
}