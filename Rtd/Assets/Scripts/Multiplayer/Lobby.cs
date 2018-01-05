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
	short SendMessageMsg = 1028;
   	short KickPlayerMsg = 1029;
    short PlayerDisconnectMsg = 1030;

    bool[] usedId = new bool[5];
    int[] cid = new int[5];
    int tcon = 0;
    LobbyPlayerData[] players = new LobbyPlayerData[5];
    NetworkConnection[] connections = new NetworkConnection[5];

    public void MapHandle(NetworkMessage netMsg){
        SetMap msg = netMsg.ReadMessage<SetMap>();
        MapIndex = msg.offset;
        SendAll(msg, SetMapMsg);
    }

    void SendAll(MessageBase msg, short messagetype){
         foreach(var conn in connections){
            if(conn != null){
                conn.Send(messagetype, msg);
            }
        }
    }

    public void AddGamePlayer(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<AddPlayerData>();
        players[msg.data.ID] = msg.data;
        SendAll(msg, AddPlayerMsg);
    }

    public void Instantiate(NetworkConnection conn, int id){
        InstantiateData msg = new InstantiateData();
        int i = 0;
        msg.players = new LobbyPlayerData[tcon];
        foreach(var player in players){
            if(player != null){
                msg.players[i++] = player;
            }
        }
        msg.mapIndex = MapIndex;
        msg.ID = id;
        conn.Send(InstantiateMsg, msg);
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn){
        //check which player has disconnected and send info to others
        Debug.Log("player disconnect");
        PlayerDisconnectData msg = new PlayerDisconnectData();
        msg.id = cid[conn.connectionId];
        usedId[msg.id] = false;
        players[msg.id] = null;
        connections[msg.id] = null;
        SendAll(msg, PlayerDisconnectMsg);
        tcon --;
    }

    public override void OnStopHost(){
        Debug.Log("host stopped");
        usedId = new bool[5];
        cid = new int[5];
        tcon = 0;
        players = new LobbyPlayerData[5];
        connections = new NetworkConnection[5];
        MapIndex = 0;
    }

    public override void OnServerConnect(NetworkConnection conn){
        int findId = GetID();
        cid[conn.connectionId] = findId;
        connections[findId] = conn;
        conn.RegisterHandler(SetMapMsg, MapHandle);
        conn.RegisterHandler(AddPlayerMsg, AddGamePlayer);
        conn.RegisterHandler(UpdatePlayerMsg, UpdatePlayer);
        conn.RegisterHandler(SendMessageMsg, DistributeMessage);
        conn.RegisterHandler(KickPlayerMsg, KickPlayer);
        base.OnServerConnect(conn);
        Instantiate(conn, findId);
        tcon ++;        
    }

    public int GetID(){
        int i =0;
        foreach(var id in usedId){
            if(!id){
                usedId[i] = true;
                return i;
            }
            i++;
        }
        return -1;
    }

    public void UpdatePlayer(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<UpdatePlayerData>();
        players[msg.data.ID] = msg.data;
        players[msg.data.ID].material = msg.data.material;
        players[msg.data.ID].cartype = msg.data.cartype;
        SendAll(msg, UpdatePlayerMsg);
    }

    public void DistributeMessage(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<MessageData>();
        SendAll(msg, SendMessageMsg);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId){
        LobbyController lc = GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>();
        int pid = cid[conn.connectionId];
        LobbyPlayerData data = players[pid];
        Transform stp = GetStartPosition();
        GameObject go =  Instantiate(lc.cars[data.cartype].car,stp, true);
        go.transform.position = stp.transform.position;
        go.transform.rotation = stp.transform.rotation;
        go.GetComponent<NetworkPlayer>().pid = pid;
        return go;
    }
    
    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
      GameObject go = Instantiate(lobbyPlayerPrefab.gameObject);
      go.GetComponent<LobbyPlayer>().ID = cid[conn.connectionId];
      return go;
    }

    public void KickPlayer(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<KickData>();
        connections[msg.id].Send(KickPlayerMsg, msg);
    }


}
