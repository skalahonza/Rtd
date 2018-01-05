using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Powerups.Shields
{
    public abstract class ShieldBase
    {
        public GameObject ShieldVisualization { get; set; }

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
            ShieldVisualization = Object.Instantiate(prefab, car.transform);
        }

        public void Clean(CarSpirit car)
        {
            Object.Destroy(ShieldVisualization);
        }

        public abstract GameObject GetPrefab();

        public abstract Sprite GetPowerupIcon();
    }
}
