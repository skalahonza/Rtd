using System;
using Boo.Lang;

namespace Assets.Scripts.Powerups
{
    public class PowerupGenerator
    {
        private readonly List<Type> _allowedPowerups;
        private readonly Random _rand = new Random();

        public PowerupGenerator()
        {
            _allowedPowerups = new List<Type>
            {
                typeof(MissilePowerup),
                //typeof(ReverseMissilePowerup),
                //typeof(MinePowerup),
            };
        }
        public IPowerup GetPowerUp()
        {
            var type = _allowedPowerups[_rand.Next(_allowedPowerups.Count)];
            return Activator.CreateInstance(type) as IPowerup;
        }
    }
}