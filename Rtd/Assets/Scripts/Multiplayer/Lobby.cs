using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Lobby : NetworkLobbyManager {

    int MapIndex;

   	short SetMapMsg = 1024;
    short UpdatePlayerMsg = 1025;
    short InstantiateMsg = 1026;
    short AddPlayerMsg = 1027;

    List<LobbyPlayerData> players = new List<LobbyPlayerData>();
    List<NetworkConnection> connections = new List<NetworkConnection>();

    public void MapHandle(NetworkMessage netMsg){
        SetMap msg = netMsg.ReadMessage<SetMap>();
        Debug.Log(string.Format("Map changed to {0}", msg.offset));
        MapIndex = msg.offset;
        SendAll(msg, SetMapMsg);
    }

    void SendAll(MessageBase msg, short messagetype){
         foreach(var conn in connections){
            conn.Send(messagetype, msg);
        }
    }

    public void AddGamePlayer(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<AddPlayerData>();
        players.Add(msg.data);
        SendAll(msg, AddPlayerMsg);
    }

    public void Instantiate(NetworkConnection conn, int id){
        InstantiateData msg = new InstantiateData();
        msg.players = players.ToArray();
        msg.mapIndex = MapIndex;
        msg.ID = id;
        conn.Send(InstantiateMsg, msg);
    }

    public void OnLobbyStartHost(){
        Debug.Log("Host Started");
    }

    public void OnLobbyStartServer(){
        Debug.Log("Server Started");
    }

    public override void OnServerConnect(NetworkConnection conn){
        connections.Add(conn);
        Debug.Log("SRV connected");
        conn.RegisterHandler(SetMapMsg, MapHandle);
        conn.RegisterHandler(AddPlayerMsg, AddGamePlayer);
        conn.RegisterHandler(UpdatePlayerMsg, UpdatePlayer);
        base.OnServerConnect(conn);
        Instantiate(conn, connections.Count);
    }

    public void UpdatePlayer(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<UpdatePlayerData>();
        players[msg.data.ID -1] = msg.data;
        SendAll(msg, UpdatePlayerMsg);
    }


    
}
