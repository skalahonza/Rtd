using UnityEngine;

namespace Assets.Scripts.Tower.Projectiles
{
    public class MissileProjectile : ProjectileBase
    {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("missile_projectile");
        }

        public override void Start()
        {                        
            base.Start();
            Speed = 10;
            Damage = 20;
        }
    }
}