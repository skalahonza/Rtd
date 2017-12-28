using System;
using Assets.Mechanics;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Powerups.Shields
{
    public class NormalShield : ShieldBase
    {
        public override bool ResolveHit(IDamageDealer dealer, CarSpirit car)
        {
            return false;
        }

        public override TimeSpan Duration
        {
            get { return TimeSpan.FromSeconds(NumberConstants.NormalShieldSeconds); }
        }

        public override void PLaySound(CarSpirit car)
        {
            var sound = SoundMechanics.SpawnSound("long_shield_sound");
            sound.transform.parent = car.transform;
            shieldSoundPlayer = sound;
        }
    }
}