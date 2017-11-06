using UnityEngine;

namespace Assets.Scripts.Tower.Projectiles
{
    public abstract class ProjectileBase:MouseHover
    {
        public float Speed;
        public float Damage;

        public abstract GameObject GetPrefab();
        

        public virtual void Start()
        {            
        }

        public virtual void Update()
        {            
        }
    }
}