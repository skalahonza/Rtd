using System;
using UnityEngine;

namespace Assets.Scripts.Powerups.Shields
{
    public abstract class ShieldBase
    {
        protected GameObject shieldSoundPlayer;

        /// <summary>
        /// Resolve hit upon the shield
        /// </summary>
        /// <param name="dealer">The damage dealer</param>
        /// <param name="car">CarSpirit that was hit</param>
        /// <returns>True if the proejctile was blocked or absorbed</returns>
        public abstract bool ResolveHit(IDamageDealer dealer, CarSpirit car);

        public abstract TimeSpan Duration { get; }

        /// <summary>
        /// Play sound on car, on which the shield was activated
        /// </summary>
        /// <param name="car">Car using the shield</param>
        public abstract void PLaySound(CarSpirit car);         

        public void Clean(CarSpirit car)
        {
            GameObject.Destroy(shieldSoundPlayer);
        }
    }
}
