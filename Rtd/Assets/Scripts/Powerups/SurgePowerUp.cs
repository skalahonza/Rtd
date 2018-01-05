using System.Collections.Generic;
using System.Linq;
using Assets.Mechanics;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups
{
    public class SurgePowerUp : PowerUpBase, IDamageDealer {  
        
        private List<IDamagable> targets = new List<IDamagable>();
        private float radius = 25;
        private float damage = 15;

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

        private IEnumerable<GameObject> SpawnEffects()
        {
            var car = gameObject.GetComponent<CarSpirit>();
            yield return AnimationMechanics.SpawnParticle("shockwave", car.gameObject.transform);
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