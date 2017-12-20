using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    public class SurgePowerupProjectile : ProjectileBase
    {
        public float BlastRadius = 50f;
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("surge_powerup_projectile");
        }

        public override void OnHit(Collider other)
        {
            if (other.gameObject.Equals(Owner)) return;
            var colliders = Physics.OverlapSphere(gameObject.transform.position, BlastRadius);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.Equals(Owner)) continue;
                var damagable = collider.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.SufferDamage(this);
                }
            }
            //TODO CONSULT WITH GAME DESIGNER THAT THE DIRECT HIT WOULD DEAL MORE DAMAGE 
            base.OnHit(other);
        }
    }
}