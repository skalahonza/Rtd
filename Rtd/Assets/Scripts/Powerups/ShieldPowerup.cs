using System.Collections.Generic;
using Assets.Scripts.Powerups.Shields;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class ShieldPowerup<T> : IPowerup
        where T :ShieldBase, new()
    {
        public List<GameObject> ObjectsToSynchronize { get; private set; }

        public ShieldPowerup()
        {
            ObjectsToSynchronize = new List<GameObject>();
        }

        public bool Use(CarSpirit car)
        {
            if (car.Shield != null)
            {
                car.Shield.Clean(car);
            }

            car.Shield = new T();
            car.Shield.Apply(car);
            ObjectsToSynchronize.Add(car.Shield.ShieldSoundPlayer);
            ObjectsToSynchronize.Add(car.Shield.ShieldVisualization);
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
        {
            //TODO CHECK IF SHIELD CAN BE USED
        }

        public Sprite GetPowerupIcon()
        {
            return new T().GetPowerupIcon();
        }    
        
        public bool UseNetwork(CarSpirit car){
            return PowerupNetworkSpawner.spawn(this, cars);
        }    
    }

}