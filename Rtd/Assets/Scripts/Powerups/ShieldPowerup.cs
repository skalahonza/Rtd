using Assets.Scripts.Powerups.Shields;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class ShieldPowerup<T> : PowerUpBase
        where T :ShieldBase, new()
    {
        public override bool Use(CarSpirit car)
        {
            if (car.Shield != null)
            {
                car.Shield.Clean(car);
            }

            car.Shield = new T();
            car.Shield.Apply(car);
            return true;
        }

        public override void UpdatePowerup(CarSpirit car)
        {
            //TODO CHECK IF SHIELD CAN BE USED
        }

        public override Sprite GetPowerupIcon()
        {
            return new T().GetPowerupIcon();
        }        
    }
}