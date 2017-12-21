using Assets.Scripts.Powerups.Shields;

namespace Assets.Scripts.Powerups
{
    public class ShieldPowerup<T> : IPowerup
        where T :ShieldBase, new()
    {
        public bool Use(CarSpirit car)
        {
            //TODO SHIELD VIZUALIZATION
            car.Shield = new T();
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
        {
            //TODO CHECK IF SHIELD CAN BE USED
        }
    }
}