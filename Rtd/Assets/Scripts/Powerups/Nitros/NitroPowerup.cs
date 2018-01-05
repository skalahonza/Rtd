using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Powerups.Nitros
{
    public class NitroPowerup<T>:IPowerup
        where T:NitroBase, new()
    {        
        public T Nitro { get; private set; }

        public List<GameObject> ObjectsToSynchronize { get; private set; }

        public NitroPowerup()
        {
            Nitro = new T();
            ObjectsToSynchronize = new List<GameObject>();
        }

        public bool UseNetwork(CarSpirit car){
            return PowerupNetworkSpawner.spawn(this, car);
        }

        public bool Spawnable(){
            return true;
        }
        
        public bool Use(CarSpirit car)
        {
            if (car.Nitro != null)
            {
                car.Nitro.Clean(car);
            }

            Nitro.OriginalMaxMotorTorque = car.MaxMotorTorque;
            car.Nitro = Nitro;
            car.Nitro.PlaySound(car);
            car.MaxMotorTorque += Nitro.SpeedBonus;
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
        {
            
        }

        public Sprite GetPowerupIcon()
        {
            return Nitro.GetPowerupIcon();
        }        
    }
}