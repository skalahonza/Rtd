using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class SetMap : MessageBase{
	public int offset;
}

public class SetName : MessageBase{
	public string name;
}


[RequireComponent(typeof(Lobby))]
public class LobbyController : NetworkBehaviour {
	public GameObject mmaker;
	bool host = false;
	public SpConfig[] maps;
	public int mapIndex = 0;
	public string name = "Player";
    public _car[] cars;

	Lobby lobby;
	short SetMapMsg = 1024;
	short SetNameMsg = 1025;
	short InstantiateMsg = 1026;
	
	public void Start(){
		mmaker.SetActive(false);
		lobby = GetComponent<Lobby>();
	}

	NetworkClient nc;
	// Use this for initialization
	public void Host () {
		host = true;
		nc = lobby.StartHost();
		nc.RegisterHandler(SetMapMsg, SetMap);
		nc.RegisterHandler(SetNameMsg, SetName);
		nc.RegisterHandler(MsgType.Connect, OnConnected);
		name = GameObject.Find("plrname").GetComponent<Text>().text;
		GameObject.Find("ConnectForm").SetActive(false);
		mmaker.SetActive(true);
		lobby.TryToAddPlayer(); 
	}
	
	// Update is called once per frame
	public void Connect () {
		host = false;
		nc = new NetworkClient();
		nc.RegisterHandler(SetMapMsg, SetMap);
		nc.RegisterHandler(SetNameMsg, SetName);
		nc.RegisterHandler(MsgType.Connect, OnConnected);
		int port = 8080;
		name = GameObject.Find("plrname").GetComponent<Text>().text;
		Int32.TryParse(GameObject.Find("port").GetComponent<Text>().text, out port );
		nc.Connect(GameObject.Find("addr").GetComponent<Text>().text, port);
	}

	public void nextMap(){
		if(maps.Length == mapIndex+1){
			return;
		}
		SetMap map = new SetMap();
		map.offset = mapIndex+1;
		nc.Send(SetMapMsg, map);
	}

	public void prevMap(){
		if(0 == mapIndex){
			return;
		}
		SetMap map = new SetMap();
		map.offset = mapIndex-1;
		nc.Send(SetMapMsg, map);
	}

   public void SetMap(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<SetMap>();
        foreach(var map in maps){
            map.image.enabled = false;
        }
		mapIndex = msg.offset;
		maps[mapIndex].image.enabled = true;
		lobby.playScene = maps[mapIndex].scene;
    }
	  
	public void SetName(NetworkMessage netMsg){
		 var msg = netMsg.ReadMessage<SetName>();
	}

	public void OnConnected(NetworkMessage netMsg){
		SetName named = new SetName();
		named.name = name;
		nc.Send(SetNameMsg, named);
    }

}
