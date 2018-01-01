using Assets.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Powerups.Nitros
{
    public class TimedNitro : NitroBase
    {
        public override int Time { get { return 25; } }
        public override int SpeedBonus { get { return 500; } }
        public override Sprite GetPowerupIcon()
        {
            return ImageMechanics.LoadSprite("timed");
        }
    }
}