using System;
using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups.Shields
{
    public class NormalShield : ShieldBase
    {
        public override bool ResolveHit(IDamageDealer dealer, CarSpirit car)
        {
            return false;
        }

        public override TimeSpan Duration
        {
            get { return TimeSpan.FromSeconds(NumberConstants.NormalShieldSeconds); }
        }

        public override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("Shields\\shield_blue");
        }

        public override Sprite GetPowerupIcon()
        {
            return ImageMechanics.LoadSprite("shield");
        }
    }
}