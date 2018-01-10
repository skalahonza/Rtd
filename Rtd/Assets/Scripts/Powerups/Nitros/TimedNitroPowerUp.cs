using UnityEngine;

namespace Assets.Scripts.Powerups.Nitros
{
    /// <summary>
    /// Apply timed nitro on a car
    /// </summary>
    public class TimedNitroPowerUp : PowerUpBase
    {
        public TimedNitro Nitro { get; private set; }

        public TimedNitroPowerUp()
        {
            Nitro = new TimedNitro();
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