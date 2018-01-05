using UnityEngine;

namespace Assets.Scripts.Powerups.Shields
{
    public class NormalShieldPowerUp : PowerUpBase
    {
        public override bool Use(CarSpirit car)
        {
            if (car.Shield != null)
            {
                car.Shield.Clean(car);
            }

            car.Shield = new NormalShield();
            car.Shield.Apply(car);
            return true;
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