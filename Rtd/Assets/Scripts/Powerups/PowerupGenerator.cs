using System;
using System.Collections.Generic;
using Assets.Scripts.Powerups.Nitros;
using Assets.Scripts.Powerups.Projectiles;
using Assets.Scripts.Powerups.Shields;

namespace Assets.Scripts.Powerups
{
    public class PowerupGenerator
    {
        private readonly List<Type> _allowedPowerups;
        private static readonly Random _rand = new Random();

        public PowerupGenerator():this(new List<Type>
        {
            typeof(MissilePowerup),
            typeof(ReverseMissilePowerup),
            typeof(MinePowerup),
            typeof(SurgePowerUp),
            typeof(NormalShieldPowerUp),
            typeof(PaybackShieldPowerUp),
            typeof(SpeedyNitrPowerUp),
            typeof(TimedNitroPowerUp)
        })
        {         
        }

        public PowerupGenerator(List<Type> allowedPowerups)
        {
            _allowedPowerups = allowedPowerups;
        }

        /// <summary>
        /// Randomly select powerup
        /// </summary>
        /// <returns>Powerup type</returns>
        public Type GetPowerUpType()
        {
            return GetPowerUpType(_rand.Next());
        }

        /// <summary>
        /// Provide rnadomized number - due to synchronization
        /// </summary>
        /// <param name="number">Nuber throwed by random generator</param>
        /// <returns>Powerup type</returns>
        public Type GetPowerUpType(int number)
        {
            return _allowedPowerups[number % _allowedPowerups.Count];
        }
    }
}