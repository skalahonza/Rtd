namespace Assets.Scripts.Powerups.Nitros
{
    public class NitroPowerup<T>:IPowerup
        where T:NitroBase, new()
    {        
        public T Nitro { get; private set; }

        public NitroPowerup()
        {
            Nitro = new T();
        }

        public bool Use(CarSpirit car)
        {
            Nitro.OriginalMaxMotorTorque = car.MaxMotorTorque;
            car.Nitro = Nitro;
            car.MaxMotorTorque += Nitro.SpeedBonus;
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
        {
        }
    }
}