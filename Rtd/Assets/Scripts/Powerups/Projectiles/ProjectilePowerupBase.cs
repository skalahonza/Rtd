using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups.Projectiles
{
    public abstract class ProjectilePowerupBase : PowerUpBase
    {
        protected GameObject Target;

        /// <summary>
        /// Range of the projectile
        /// </summary>
        public float Range = 75;

        protected abstract GameObject GetProjectilePrefab();

        /// <inheritdoc />
        /// <summary>
        /// Use the powerup and apply it's damage or debuff upon target/s
        /// </summary>
        /// <param name="car">Owner of the powerup</param>
        /// <returns>True if action performed successfully</returns>
        public override bool Use(CarSpirit car)
        {
            if (Target == null) return false;     
            SpawnProjectile();
            return true;
        }

        private GameObject SpawnProjectile()
        {
            var car = gameObject.GetComponent<CarSpirit>();
            // create projectile instance
            //spawn and fire projectile
            var prefab = GetProjectilePrefab();
            var position = NumberConstants.MineSpawnHeight(car.transform.position);
            var projectile = Instantiate(prefab, position,
                TargetingMechanis.CalculateAimRotation(Target.transform.position, car.transform.position));

            var projBase = projectile.GetComponent<ProjectileBase>();
            projBase.Owner = car.gameObject;

            var velocity = TargetingMechanis.CalculateAimVelocityVector(Target.transform, car.transform.position, projBase.Speed);

            projectile.GetComponent<Rigidbody>().velocity = velocity;
            return projectile;
        }

        public bool Spawnable(){
            return true;
        }
        
        /// <summary>
        /// Updates powerup targets and other properties if required
        /// </summary>
        /// <param name="car">Powerup owner</param>
        public override void UpdatePowerup(CarSpirit car)
        {
            LockTarget(car.gameObject.transform.position, car.transform.rotation);
        }

        public abstract override Sprite GetPowerupIcon();

        /// <summary>
        /// Lock on target from current position and rotation
        /// </summary>
        /// <param name="center">Shooter's position</param>
        /// <param name="rotation">Shooter's rotation</param>
        /// <returns></returns>
        public abstract GameObject LockTarget(Vector3 center, Quaternion rotation);

        /// <summary>
        /// Check if new target is in range, if no new target found, check if old target is still in range
        /// </summary>
        /// <param name="target">New target, that was detected</param>
        /// <param name="direction">Direction of aiming, typically rotation*vector3.forward</param>
        /// <param name="center">Shooter's position</param>
        protected virtual void NewTrgetFound(GameObject target, Vector3 direction, Vector3 center)
        {
            bool previouslyEmptyTarget = false;
            // new target detected
            if (target != null)
            {
                previouslyEmptyTarget = Target == null;
                Target = target;
            }

            // verify old or new target
            if (Target != null)
            {
                // target not in range
                if (!TargetingMechanis.IsTargetInRange(Target, direction, center, Range, NumberConstants.DetetionAngle))
                {
                    Target = null;
                }
                else
                {
                    // TODO draw rectangle arround locked target                    
                    Debug.DrawRay(center, Target.transform.position - center, Color.green);

                    if (previouslyEmptyTarget)
                    {
                        if (isLocalPlayer || !MultiplayerHelper.IsMultiplayer())
                            if (gameObject.GetComponent<AIPlayer>() == null)
                                SoundMechanics.SpawnSound("radar_sound");
                    }
                }
            }
        }
    }
}