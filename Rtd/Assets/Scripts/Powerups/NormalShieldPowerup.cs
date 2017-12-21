using Assets.Scripts.Powerups.Shields;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class NormalShieldPowerup : IPowerup {
        public bool Use(CarSpirit car)
        {
            //TODO SHIELD VIZUALIZATION
            car.Shield = new NormalShield();
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
        {
            //TODO CHECK IF SHIELD CAN BE USED
        }
    }
}