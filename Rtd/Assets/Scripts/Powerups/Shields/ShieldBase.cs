using System;
using UnityEngine;

namespace Assets.Scripts.Powerups.Shields
{
    public abstract class ShieldBase
    {
        protected GameObject shieldSoundPlayer;
        protected GameObject shieldVisualization;

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
        public virtual void Apply(CarSpirit car)
        {
            var prefab = GetPrefab();
            shieldVisualization = GameObject.Instantiate(prefab, car.transform);
        }

        public void Clean(CarSpirit car)
        {
            GameObject.Destroy(shieldVisualization);
            GameObject.Destroy(shieldSoundPlayer);
        }

        protected abstract GameObject GetPrefab();
    }
}
