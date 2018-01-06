using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Mechanics
{
    public static class MultiplayerHelper
    {
        public static bool IsMultiplayer()
        {
            //TODO SLAMAAAA FIX IT
            bool multi = Network.isClient || Network.isServer || NetworkServer.active;
            return multi;
        }
    }
}