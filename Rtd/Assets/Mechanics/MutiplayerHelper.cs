using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Mechanics
{
    public static class MultiplayerHelper
    {
        public static bool IsMultiplayer()
        {
            bool multi = Network.isClient || Network.isServer || NetworkServer.active;
            return multi;
        }
    }
}