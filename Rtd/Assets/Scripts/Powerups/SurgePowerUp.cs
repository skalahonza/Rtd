using System.Collections.Generic;
using System.Linq;
using Assets.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class SurgePowerUp : IPowerup, IDamageDealer {  
        
        private List<IDamagable> targets = new List<IDamagable>();
        private float radius = 25;
        private float damage = 15;

        public bool Use(CarSpirit car)
        {
            if (!targets.Any()) return false;

            SoundMechanics.SpawnSound("surge_sound");
            AnimationMechanics.SpawnParticle("shockwave", car.gameObject.transform);
            foreach (var target in targets)
            {
                target.SufferDamage(this);
            }
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
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

        public Sprite GetPowerupIcon()
        {
            throw new System.NotImplementedException();
        }

        public void DealDamage(CarSpirit car)
        {
            car.Hp -= damage;
            //TODO DISABLE HUD AND POWERUPS
        }
    }
}