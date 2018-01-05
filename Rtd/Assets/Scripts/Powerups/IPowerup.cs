using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public interface IPowerup
    {
        /// <summary>
        /// When the powerup is used by the player
        /// </summary>
        bool Use(CarSpirit car);
        /// <summary>
        /// When the powerup is used by the player
        /// </summary>
        bool UseNetwork(CarSpirit car);
        /// <summary>
        /// Update powerup data, retarget or verify that the powerup can be used
        /// </summary>
        /// <param name="car">Onwer of the powerup</param>
        void UpdatePowerup(CarSpirit car);
        /// <summary>
        /// Get powerup icon for each powerup
        /// </summary>
        /// <returns>Sprite of powerup</returns>
        Sprite GetPowerupIcon();
    }
}