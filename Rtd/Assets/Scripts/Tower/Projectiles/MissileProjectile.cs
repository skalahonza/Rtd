using UnityEngine;

namespace Assets.Scripts.Tower.Projectiles
{
    public class MissileProjectile : ProjectileBase
    {
        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("missile_projectil");
        }

        public override void Start()
        {                        
            base.Start();
            Speed = 1;
            Damage = 10;
        }
    }
}