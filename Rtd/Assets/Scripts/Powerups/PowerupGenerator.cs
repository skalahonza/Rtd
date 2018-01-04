using System;
using System.Collections.Generic;
using Assets.Scripts.Powerups.Nitros;
using Assets.Scripts.Powerups.Shields;

namespace Assets.Scripts.Powerups
{
    public class PowerupGenerator
    {
        private readonly List<Type> _allowedPowerups;
        private readonly Random _rand = new Random();

        public PowerupGenerator():this(new List<Type>
        {
            typeof(MissilePowerup),
            typeof(ReverseMissilePowerup),
            typeof(MinePowerup),
            typeof(SurgePowerUp),
            typeof(ShieldPowerup<NormalShield>),
            typeof(ShieldPowerup<PaybackShield>),        
            typeof(NitroPowerup<SpeedyNitro>),        
            typeof(NitroPowerup<TimedNitro>),           
        })
        {         
        }

        public PowerupGenerator(List<Type> allowedPowerups)
        {
            _allowedPowerups = allowedPowerups;
        }

        public IPowerup GetPowerUp()
        {
            var type = _allowedPowerups[_rand.Next(_allowedPowerups.Count)];
            return Activator.CreateInstance(type) as IPowerup;
        }

        public Type GetPowerUpType()
        {
            return _allowedPowerups[_rand.Next(_allowedPowerups.Count)];
        }
    }
}