using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups
{
    public abstract class PowerUpBase : NetworkBehaviour,IPowerup
    {
        public abstract bool Use(CarSpirit car);
        public abstract void UpdatePowerup(CarSpirit car);
        public abstract Sprite GetPowerupIcon();
    }
}