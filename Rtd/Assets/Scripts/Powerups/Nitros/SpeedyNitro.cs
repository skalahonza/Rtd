using Assets.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Powerups.Nitros
{
    public class SpeedyNitro : NitroBase
    {
        public override int Time { get { return 15; } }
        public override int SpeedBonus { get { return 1000; } }
        public override Sprite GetPowerupIcon()
        {
            return ImageMechanics.LoadSprite("speedy");
        }
    }
}