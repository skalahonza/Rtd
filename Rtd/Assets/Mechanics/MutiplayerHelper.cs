using UnityEngine;
using UnityEngine.Networking;

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
            return GameObject.FindObjectOfType<NetworkPlayer>() != null;
        }
    }
}