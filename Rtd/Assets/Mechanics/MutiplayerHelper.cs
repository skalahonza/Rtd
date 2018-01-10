using UnityEngine;

namespace Assets.Mechanics
{
    public static class MultiplayerHelper
    {
        /// <summary>
        /// Check if the current game is multiplayer
        /// </summary>
        /// <returns></returns>
        public static bool IsMultiplayer()
        {
            return Object.FindObjectOfType<NetworkPlayer>() != null;
        }
    }
}