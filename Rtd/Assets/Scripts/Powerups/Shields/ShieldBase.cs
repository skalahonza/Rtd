using System;

namespace Assets.Scripts.Powerups.Shields
{
    public abstract class ShieldBase
    {
        /// <summary>
        /// Resolve hit upon the shield
        /// </summary>
        /// <param name="dealer">The damage dealer</param>
        /// <param name="car">CarSpirit that was hit</param>
        /// <returns>True if the proejctile was blocked or absorbed</returns>
        public abstract bool ResolveHit(IDamageDealer dealer, CarSpirit car);

        public abstract TimeSpan Duration { get; }
    }
}