using System;
using UnityEngine;

namespace Assets.Scripts.Powerups.Shields
{
    public abstract class ShieldBase
    {
        public GameObject ShieldSoundPlayer { get; protected set; }
        public GameObject ShieldVisualization { get; protected set; }

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
            ShieldVisualization = GameObject.Instantiate(prefab, car.transform);
        }

        public void Clean(CarSpirit car)
        {
            GameObject.Destroy(ShieldVisualization);
            GameObject.Destroy(ShieldSoundPlayer);
        }

        protected abstract GameObject GetPrefab();

        public abstract Sprite GetPowerupIcon();
    }
}
