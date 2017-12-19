using UnityEngine;

namespace Assets.Scripts.Tower.Projectiles
{
    public class MissileProjectile : ProjectileBase
    {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("tower_projectile_orange");
        }

        public override void OnHit(Collider other)
        {
            var parent = other.transform.gameObject;
            IDamagable target;
            if ((target = parent.GetComponent<IDamagable>()) != null)
            {
                target.SufferDamage(this);
            }
            base.OnHit(other);
        }
    }   
}