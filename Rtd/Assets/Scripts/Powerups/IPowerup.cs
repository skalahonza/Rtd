namespace Assets.Scripts.Powerups
{
    public interface IPowerup
    {
        /// <summary>
        /// When the powerup is used by the player
        /// </summary>
        bool Use(CarSpirit car);
        /// <summary>
        /// Update powerup data, 
        /// </summary>
        /// <param name="car">Onwer of the powerup</param>
        void UpdatePowerup(CarSpirit car);
    }
}