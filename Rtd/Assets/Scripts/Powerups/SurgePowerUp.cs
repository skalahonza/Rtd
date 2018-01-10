using System.Collections.Generic;
using System.Linq;
using Assets.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    /// <summary>
    /// Apply surge powerup on a car
    /// </summary>
    public class SurgePowerUp : PowerUpBase, IDamageDealer {  
        
        private readonly List<IDamagable> targets = new List<IDamagable>();
        private readonly float radius = 25;
        private readonly float damage = 15;

        public override bool Use(CarSpirit car)
        {
            if (!targets.Any()) return false;
            SpawnEffects();

            foreach (var target in targets)
            {
                target.SufferDamage(this);
            }
            return true;
        }

        public override void UpdatePowerup(CarSpirit car)
        {
            targets.Clear();
            var colliders = Physics.OverlapSphere(car.transform.position, radius);
            foreach (var collider in colliders)
            {
                if (collider.gameObject == car.gameObject) continue;

                var damagable = collider.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    targets.Add(damagable);
                }
            }
        }

        private void SpawnEffects()
        {
            var car = gameObject.GetComponent<CarSpirit>();
            AnimationMechanics.SpawnParticle("shockwave", car.gameObject.transform);
        }

        public override Sprite GetPowerupIcon()
        {
            return ImageMechanics.LoadSprite("surge");
        }

        public void DealDamage(CarSpirit car)
        {
            car.Hp -= damage;
            car.SilencePowerup();
        }
    }
}