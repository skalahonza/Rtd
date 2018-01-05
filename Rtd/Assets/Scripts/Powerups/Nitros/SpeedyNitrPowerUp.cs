using UnityEngine;

namespace Assets.Scripts.Powerups.Nitros
{
    public class SpeedyNitrPowerUp : PowerUpBase
    {
        public SpeedyNitro Nitro { get; private set; }

        public SpeedyNitrPowerUp()
        {
            Nitro = new SpeedyNitro();
        }

        public override bool Use(CarSpirit car)
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

        public override void UpdatePowerup(CarSpirit car)
        {

        }

        public override Sprite GetPowerupIcon()
        {
            return Nitro.GetPowerupIcon();
        }
    }
}