using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("OnPlayerConnected");
    }
}