namespace Assets.Scripts.Powerups.Nitros
{
    public abstract class NitroBase
    {
        public float OriginalMaxMotorTorque { get; set; }
        public abstract int Time { get; }
        public abstract int SpeedBonus { get; }
    }
}