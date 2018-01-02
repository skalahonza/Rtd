using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Lobby : NetworkLobbyManager {

    public GameObject spawn;

   	short SetMapMsg = 1024;
    short SetNameMsg = 1025;

    List<NetworkConnection> connections = new List<NetworkConnection>();

    public void MapHandle(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<SetMap>();
        foreach(var conn in connections){
            conn.Send(SetMapMsg, msg);
        }
    }
    
    public void NameHandle(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<SetName>();
        //GameObject go = Instantiate(lobbyPlayerPrefab, GameObject.Find("player_setup").transform).gameObject;
        //go.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg.name; 
        foreach(var conn in connections){
            conn.Send(SetNameMsg, msg);
        }
    }

    public override void OnServerConnect(NetworkConnection conn){
        conn.RegisterHandler(SetMapMsg, MapHandle);
        conn.RegisterHandler(SetNameMsg, NameHandle);
        connections.Add(conn);
     }

    //handle player spawning
}
