﻿using Assets.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public abstract class ProjectilePowerupBase : IPowerup
    {
        protected GameObject Target;

        /// <summary>
        /// Range of the projectile
        /// </summary>
        public float Range = 75;

        public ProjectileBase Projectile;

        /// <inheritdoc />
        /// <summary>
        /// Use the powerup and apply it's damage or debuff upon target/s
        /// </summary>
        /// <param name="car">Owner of the powerup</param>
        /// <returns>True if action performed successfully</returns>
        public bool Use(CarSpirit car)
        {
            if (Target == null) return false;
            
            // create projectile instance
            //spawn and fire projectile
            var projectile = GameObject.Instantiate(Projectile.GetPrefab(), car.transform.position,
                TargetingMechanis.CalculateAimRotation(Target.transform.position, car.transform.position));

            var projBase = projectile.GetComponent<ProjectileBase>();
            projBase.Owner = car.gameObject;

            var velocity =
                TargetingMechanis.CalculateAimVelocityVector(Target.transform, car.transform.position, projBase.Speed);
            projectile.GetComponent<Rigidbody>().velocity = velocity;
            // TODO audio play 
            return true;
        }

        /// <summary>
        /// Updates powerup targets and other properties if required
        /// </summary>
        /// <param name="car">Powerup owner</param>
        public void UpdatePowerup(CarSpirit car)
        {
            Projectile.Owner = car.gameObject;
            LockTarget(car.gameObject.transform.position, car.transform.rotation);            
        }

        public abstract GameObject LockTarget(Vector3 center, Quaternion rotation);
    }
}