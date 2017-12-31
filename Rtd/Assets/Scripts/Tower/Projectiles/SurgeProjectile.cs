using UnityEngine;

namespace Assets.Scripts.Tower.Projectiles
{
    public class SurgeProjectile : ProjectileBase
    {
        public float BlastRadius = 50f;
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("surge_projectile");
        }

        public override void OnHit(Collider other)
        {
            var colliders = Physics.OverlapSphere(gameObject.transform.position, BlastRadius);
            foreach (var collider in colliders)
            {
                var damagable = collider.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.SufferDamage(this);
                }
            }
            base.OnHit(other);
        }
    }
}