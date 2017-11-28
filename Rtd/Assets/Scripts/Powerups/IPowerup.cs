namespace Assets.Scripts.Powerups
{
    public interface IPowerup
    {
        /// <summary>
        /// When the powerup is used by the player
        /// </summary>
        void Use();
        /// <summary>
        /// Update powerup data, 
        /// </summary>
        /// <param name="car">Onwer of the powerup</param>
        void UpdatePowerup(CarSpirit car);
    }
}