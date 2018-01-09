using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// lobby manager server side
/// </summary>
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

    /// <summary>
    /// handle map changing packet
    /// </summary>
    /// <param name="netMsg"></param>
    public void MapHandle (NetworkMessage netMsg) {
        SetMap msg = netMsg.ReadMessage<SetMap> ();
        MapIndex = msg.offset;
        SendAll (msg, SetMapMsg);
    }

    /// <summary>
    /// send packet to all clients
    /// </summary>
    /// <param name="msg">message data</param>
    /// <param name="messagetype">message type</param>
    void SendAll (MessageBase msg, short messagetype) {
        foreach (var conn in connections) {
            if (conn != null) {
                conn.Send (messagetype, msg);
            }
        }
    }

    /// <summary>
    /// Handle adding player on server
    /// </summary>
    /// <param name="netMsg"></param>
    public void AddGamePlayer (NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<AddPlayerData> ();
        players[msg.data.ID] = msg.data;
        SendAll (msg, AddPlayerMsg);
    }

    /// <summary>
    /// instantiate connection send all neccessary data and wait for addplayer
    /// </summary>
    /// <param name="conn">
    /// connection 
    /// </param>
    /// <param name="id">
    /// player ID
    /// </param>
    public void Instantiate (NetworkConnection conn, int id) {
        InstantiateData msg = new InstantiateData ();
        int i = 0;
        msg.players = new LobbyPlayerData[tcon];
        foreach (var player in players) {
            if (player != null) {
                msg.players[i++] = player;
            }
        }
        msg.mapIndex = MapIndex;
        msg.ID = id;
        conn.Send (InstantiateMsg, msg);
    }

    /// <summary>
    /// Handle player disconnection
    /// </summary>
    /// <param name="conn">
    /// connection
    /// </param>
    public override void OnLobbyServerDisconnect (NetworkConnection conn) {
        //check which player has disconnected and send info to others
        Debug.Log ("player disconnect");
        PlayerDisconnectData msg = new PlayerDisconnectData ();
        msg.id = cid[conn.connectionId];
        usedId[msg.id] = false;
        players[msg.id] = null;
        connections[msg.id] = null;
        SendAll (msg, PlayerDisconnectMsg);
        tcon--;
    }

    /// <summary>
    /// handle host stopping - clear data
    /// game object (networkmanager singleton cannot be destroyed due to bug 712042)
    /// </summary>
    public override void OnStopHost () {
        Debug.Log ("host stopped");
        usedId = new bool[5];
        cid = new int[5];
        tcon = 0;
        players = new LobbyPlayerData[5];
        connections = new NetworkConnection[5];
        MapIndex = 0;
    }

    /// <summary>
    /// handle new connections
    /// </summary>
    /// <param name="conn">
    /// connection
    /// </param>
    public override void OnServerConnect (NetworkConnection conn) {
        int findId = GetID ();
        cid[conn.connectionId] = findId;
        connections[findId] = conn;
        conn.RegisterHandler (SetMapMsg, MapHandle);
        conn.RegisterHandler (AddPlayerMsg, AddGamePlayer);
        conn.RegisterHandler (UpdatePlayerMsg, UpdatePlayer);
        conn.RegisterHandler (SendMessageMsg, DistributeMessage);
        conn.RegisterHandler (KickPlayerMsg, KickPlayer);
        base.OnServerConnect (conn);
        Instantiate (conn, findId);
        tcon++;
    }

    /// <summary>
    /// get new player ID
    /// </summary>
    /// <returns>
    /// ID
    /// </returns>
    public int GetID () {
        int i = 0;
        foreach (var id in usedId) {
            if (!id) {
                usedId[i] = true;
                return i;
            }
            i++;
        }
        return -1;
    }

    /// <summary>
    /// update Player data (car type)
    /// </summary>
    /// <param name="netMsg"></param>
    public void UpdatePlayer (NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<UpdatePlayerData> ();
        players[msg.data.ID] = msg.data;
        players[msg.data.ID].material = msg.data.material;
        players[msg.data.ID].cartype = msg.data.cartype;
        SendAll (msg, UpdatePlayerMsg);
    }

    /// <summary>
    /// distribute chat message
    /// </summary>
    /// <param name="netMsg"></param>
    public void DistributeMessage (NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<MessageData> ();
        SendAll (msg, SendMessageMsg);
    }

    /// <summary>
    /// create game objects for player
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="playerControllerId"></param>
    /// <returns>
    /// new gameobject
    /// </returns>
    public override GameObject OnLobbyServerCreateGamePlayer (NetworkConnection conn, short playerControllerId) {
        LobbyController lc = GameObject.FindGameObjectsWithTag ("network") [0].GetComponent<LobbyController> ();
        int pid = cid[conn.connectionId];
        LobbyPlayerData data = players[pid];
        Transform stp = GetStartPosition ();
        GameObject go = Instantiate (lc.cars[data.cartype].car, stp, true);
        go.transform.position = stp.transform.position;
        go.transform.rotation = stp.transform.rotation;
        go.GetComponent<NetworkPlayer> ().pid = pid;
        go.GetComponent<NetworkPlayer> ().cid = pid;
        go.GetComponent<NetworkPlayer> ().cname = data.cname;
        return go;
    }

    /// <summary>
    /// hndle connecting to lobby and ceating object (setting ID)
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="playerControllerId"></param>
    /// <returns></returns>
    public override GameObject OnLobbyServerCreateLobbyPlayer (NetworkConnection conn, short playerControllerId) {
        GameObject go = Instantiate (lobbyPlayerPrefab.gameObject);
        go.GetComponent<LobbyPlayer> ().ID = cid[conn.connectionId];
        return go;
    }

    /// <summary>
    /// handle kicking player
    /// </summary>
    /// <param name="netMsg"></param>
    public void KickPlayer (NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<KickData> ();
        connections[msg.id].Send (KickPlayerMsg, msg);
    }
}