using Assets.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Powerups.Nitros
{
    /// <summary>
    /// Base nitro object for TimedNitro SpeedyNitro
    /// </summary>
    public abstract class NitroBase
    {
        private GameObject nitroSoundPlayer;
        public float OriginalMaxMotorTorque { get; set; }
        public abstract int Time { get; }
        public abstract int SpeedBonus { get; }

        public abstract Sprite GetPowerupIcon();

        /// <summary>
        /// Clean nitro from car
        /// </summary>
        /// <param name="car">Car to be clean</param>
        public void Clean(CarSpirit car)
        {
            Object.Destroy(nitroSoundPlayer);
            car.MaxMotorTorque = OriginalMaxMotorTorque;
        }

        /// <summary>
        /// Play nitro sound on car
        /// </summary>
        /// <param name="car">Car to be played on</param>
        public void PlaySound(CarSpirit car)
        {
            var sound = SoundMechanics.SpawnSound("nitro_sound", car.transform);
            nitroSoundPlayer = sound;
        }
    }
}