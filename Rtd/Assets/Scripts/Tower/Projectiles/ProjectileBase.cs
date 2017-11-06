using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Tower.Projectiles
{
    public abstract class ProjectileBase:MouseHover
    {
        public float Speed;
        public float Damage;

        public abstract GameObject GetPrefab();

        public virtual void OnHit()
        {
            Destroy(gameObject);
        }

        public virtual void Start()
        {            
        }

        public virtual void Update()
        {            
        }

        public virtual void OnTriggerEnter(Collider other)
        {            
            //OnDestroy
            if(!other.CompareTag(GameTag.Tower.ToString()) && !other.CompareTag(GameTag.Muzzle.ToString()))
                OnHit();

            
            if (other.CompareTag(GameTag.Player.ToString()))
            {
                //TODO Deal damage
            }
        }
    }
}