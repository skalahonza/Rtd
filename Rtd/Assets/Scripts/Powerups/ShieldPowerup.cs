using Assets.Scripts.Powerups.Shields;

namespace Assets.Scripts.Powerups
{
    public class ShieldPowerup<T> : IPowerup
        where T :ShieldBase, new()
    {
        public bool Use(CarSpirit car)
        {
            //TODO SHIELD VIZUALIZATION
            if (car.Shield != null)
            {
                car.Shield.Clean(car);
            }

            car.Shield = new T();
            car.Shield.PLaySound(car);
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
        {
            //TODO CHECK IF SHIELD CAN BE USED
        }
    }
}