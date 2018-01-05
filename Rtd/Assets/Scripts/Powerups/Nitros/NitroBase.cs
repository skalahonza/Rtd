using Assets.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Powerups.Nitros
{
    public abstract class NitroBase
    {
        private GameObject nitroSoundPlayer;
        public float OriginalMaxMotorTorque { get; set; }
        public abstract int Time { get; }
        public abstract int SpeedBonus { get; }

        public abstract Sprite GetPowerupIcon();

        public void Clean(CarSpirit car)
        {
            Object.Destroy(nitroSoundPlayer);
            car.MaxMotorTorque = OriginalMaxMotorTorque;
        }

        public void PlaySound(CarSpirit car)
        {
            var sound = SoundMechanics.SpawnSound("nitro_sound");
            sound.transform.parent = car.transform;
            nitroSoundPlayer = sound;
        }
    }
}