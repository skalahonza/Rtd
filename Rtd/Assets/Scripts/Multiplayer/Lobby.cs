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

    int loaded = 0;

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

    public override void OnLobbyStartHost(){
        Debug.Log("Host Started");
    }

    public override void OnLobbyStartServer(){
        Debug.Log("Server Started");
    }
    
    public override void OnLobbyServerPlayersReady(){
        string sceneName = GameObject.Find("network").GetComponent<LobbyController>().maps[MapIndex].scene;
        Application.LoadLevel(sceneName);
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer){
        loaded++;
        return true;
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
        players[msg.data.ID -1].material = msg.data.material;
        players[msg.data.ID -1].cartype = msg.data.cartype;
        SendAll(msg, UpdatePlayerMsg);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId){
        LobbyController lc = GameObject.Find("network").GetComponent<LobbyController>();
        LobbyPlayerData data = players[playerControllerId];
        GameObject go =  Instantiate(lc.cars[data.cartype].car);
        Material material = lc.cars[data.cartype].materials[data.material];
        go.transform.GetChild(0).GetComponent<Renderer>().material = material;
        go.transform.GetChild(1).GetComponent<Renderer>().material = material;
        go.transform.GetChild(2).GetComponent<Renderer>().material = material;
        go.transform.GetChild(3).GetComponent<Renderer>().material = material;
        go.transform.GetChild(4).GetComponent<Renderer>().material = material;
        return go;
    }

}
