using System;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Powerups.Shields
{
    public class NormalShield : ShieldBase
    {
        public override bool ResolveHit(IDamageDealer dealer, CarSpirit carSpirit)
        {
            return false;
        }

        public override TimeSpan Duration
        {
            get { return TimeSpan.FromSeconds(NumberConstants.NormalShieldSeconds); }
        }
    }    
}