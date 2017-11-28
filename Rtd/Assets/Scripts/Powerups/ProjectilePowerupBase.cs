using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public abstract class ProjectilePowerupBase : MonoBehaviour,IPowerup
    {
        protected GameObject Target;

        /// <summary>
        /// Range of the projectile
        /// </summary>
        public float Range = 75;

        public ProjectileBase Projectile;
        
        /// <summary>
        /// Updates powerup targets and other properties if required
        /// </summary>
        /// <param name="car">Powerup owner</param>
        public void UpdatePowerup(CarSpirit car)
        {
            var target = LockTarget(car.gameObject.transform.position, car.transform.rotation);
            // TODO draw rectangle arround locked target
            
            if (target != null)
            {
                Target = target;                
            }

            if (Target != null && (Target.transform.position - car.transform.position).magnitude <= Range)
            {
                Debug.DrawRay(car.transform.position, Target.transform.position - car.transform.position, Color.green);
            }
        }

        public void Use()
        {
            
        }

        public abstract GameObject LockTarget(Vector3 center, Quaternion rotation);
    }
}