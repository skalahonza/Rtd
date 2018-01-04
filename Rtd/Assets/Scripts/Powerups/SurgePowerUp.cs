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

        public List<GameObject> ObjectsToSynchronize { get; private set; }

        public SurgePowerUp()
        {
            ObjectsToSynchronize = new List<GameObject>();
        }

        public bool Use(CarSpirit car)
        {
            if (!targets.Any()) return false;

            var sound = SoundMechanics.SpawnSound("surge_sound");
            var effect = AnimationMechanics.SpawnParticle("shockwave", car.gameObject.transform);
            ObjectsToSynchronize.Add(sound);
            ObjectsToSynchronize.Add(effect);

            foreach (var target in targets)
            {
                target.SufferDamage(this);
            }
            return true;
        }

        public bool UseNetwork(CarSpirit car){
            return PowerupNetworkSpawner.spawn(this, cars);
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
            return ImageMechanics.LoadSprite("surge");
        }

        public void DealDamage(CarSpirit car)
        {
            car.Hp -= damage;
            //TODO DISABLE HUD AND POWERUPS
        }
    }
}