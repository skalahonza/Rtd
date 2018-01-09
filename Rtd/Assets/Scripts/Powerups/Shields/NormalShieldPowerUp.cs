using System.Collections.Generic;
using Assets.Mechanics;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups.Shields
{
    public class NormalShieldPowerUp : PowerUpBase
    {
        public override bool Use(CarSpirit car)
        {              
            //apply shield to car
            SpawnEffects();
            return true;
        }

        private void SpawnEffects()
        {
            var car = gameObject.GetComponent<CarSpirit>();

            if (car.Shield != null)
            {
                car.Shield.Clean(car);
            }

            car.Shield = new NormalShield();
            var prefab = car.Shield.GetPrefab();
            car.Shield.ShieldVisualization = Instantiate(prefab, car.transform);
        }

        public override void UpdatePowerup(CarSpirit car)
        {
            //TODO CHECK IF SHIELD CAN BE USED
        }

        public override Sprite GetPowerupIcon()
        {
            return new NormalShield().GetPowerupIcon();
        }
    }
}