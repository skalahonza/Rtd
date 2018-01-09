using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Mechanics
{
    public static class MultiplayerHelper
    {
        public static bool IsMultiplayer()
        {
            return GameObject.FindObjectOfType<NetworkPlayer>() != null;
        }
    }
}