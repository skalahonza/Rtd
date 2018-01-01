using Assets.Mechanics;
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
            var particle = AnimationMechanics.SpawnParticle("shockwave_projectile_effect");
            particle.transform.position = gameObject.transform.position;
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