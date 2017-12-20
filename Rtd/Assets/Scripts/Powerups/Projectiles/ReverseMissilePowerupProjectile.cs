using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    public class ReverseMissilePowerupProjectile : ProjectileBase {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("reverse_missile_projectile");
        }

        public override void OnHit(Collider other)
        {
            var parent = other.transform.gameObject;
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