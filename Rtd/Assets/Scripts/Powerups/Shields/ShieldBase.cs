using System;

namespace Assets.Scripts.Powerups.Shields
{
    public abstract class ShieldBase
    {
        /// <summary>
        /// Resolve hit upon the shield
        /// </summary>
        /// <param name="dealer">The damage dealer</param>
        /// <param name="carSpirit">CarSpirit that was hit</param>
        /// <returns>True if the proejctile was blocked or absorbed</returns>
        public abstract bool ResolveHit(IDamageDealer dealer, CarSpirit carSpirit);

        public abstract TimeSpan Duration { get; }
    }
}