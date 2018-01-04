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

public class InstantiateData :MessageBase {
	public LobbyPlayerData[] players;
	public int mapIndex ;
	public int ID;
}

public class AddPlayerData : MessageBase{
	public LobbyPlayerData data;
}

public class UpdatePlayerData : MessageBase{
	public LobbyPlayerData data;
}

public class StartGame : MessageBase {

}

[RequireComponent(typeof(Lobby))]
public class LobbyController : NetworkBehaviour {
	public GameObject spawn;
    public GameObject spawnObject;	
	public GameObject mmaker;
	bool host = false;
	public SpConfig[] maps;
	public int mapIndex = 0;
	public string cname = "Player";
    public _car[] cars;
	GameObject[] playerSetupObj = new GameObject[5];

	bool bug1 = false;
	Lobby lobby;
	short SetMapMsg = 1024;
	short UpdatePlayerMsg = 1025;
	short AddPlayerMsg = 1027;
	short InstantiateMsg = 1026;
	NetworkClient nc;
	int myID;
	public LobbyPlayerData[] myData = new LobbyPlayerData[5];
	
	public void Start(){
		lobby = GetComponent<Lobby>();
		mmaker.SetActive(false);
	}

	public void Host () {
		host = true;
		nc = lobby.StartHost();
		InitializeNetworkClient(nc);
		cname = GameObject.Find("plrname").GetComponent<Text>().text;
		GameObject.Find("ConnectForm").SetActive(false);
		mmaker.SetActive(true);
	}
	
	public void Connect () {
		host = false;
		int port = 7777;
		cname = GameObject.Find("plrname").GetComponent<Text>().text;
		Int32.TryParse(GameObject.Find("port").GetComponent<Text>().text, out port );
		lobby.networkPort = port;
		lobby.networkAddress = GameObject.Find("addr").GetComponent<Text>().text;
		nc = lobby.StartClient(); 
		InitializeNetworkClient(nc);
		GameObject.Find("ConnectForm").SetActive(false);
		mmaker.SetActive(true);
	}

	void InitializeNetworkClient(NetworkClient nc){
		nc.RegisterHandler(SetMapMsg, SetMap);
		nc.RegisterHandler(AddPlayerMsg, AddPlayer);
		nc.RegisterHandler(UpdatePlayerMsg, UpdatePlayer);
		nc.RegisterHandler(InstantiateMsg, OnConnected);
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
		mapIndex = msg.offset;
		ResetMap();
    }

	void ResetMap(){
		foreach(var map in maps){
            map.image.enabled = false;
        }
		maps[mapIndex].image.enabled = true;
		lobby.playScene = maps[mapIndex].scene;
	}
	  
	public void AddPlayer(NetworkMessage netMsg){
		var msg = netMsg.ReadMessage<AddPlayerData>();
		GameObject go = Instantiate(spawnObject, spawn.transform).gameObject;
        go.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg.data.cname; 
		if(myData[myID].ID != msg.data.ID){
			go.transform.GetChild(1).gameObject.GetComponent<Dropdown>().interactable = false;
		}
		playerSetupObj[msg.data.ID - 1] = go;
		myData[msg.data.ID - 1]  = msg.data;
	}

	public void OnConnected(NetworkMessage netMsg){
		InstantiateData inst = netMsg.ReadMessage<InstantiateData>();
		mapIndex = inst.mapIndex;
		ResetMap();
		AddPlayerData msg = new AddPlayerData();
		msg.data = new LobbyPlayerData();
		myID = msg.data.ID = inst.ID;
		msg.data.material = inst.ID - 1;
		msg.data.cname = cname;
		myData[msg.data.ID] = msg;
		msg.data.cartype = 0;
		nc.Send(AddPlayerMsg, msg);
		foreach(var plr in inst.players){
			GameObject go = Instantiate(spawnObject, spawn.transform).gameObject;
        	go.transform.GetChild(0).gameObject.GetComponent<Text>().text = plr.cname;
			go.transform.GetChild(1).gameObject.GetComponent<Dropdown>().interactable = false;
			bug1 = true;
     		go.transform.GetChild(1).gameObject.GetComponent<Dropdown>().value = plr.cartype;
			bug1 = false;
			playerSetupObj[plr.ID - 1] = go;
			myData[plr.ID - 1]  = plr;
		}
    }

	public void UpdatePlayer(NetworkMessage netMsg){
		var msg = netMsg.ReadMessage<UpdatePlayerData>();
		bug1 = true;
		playerSetupObj[msg.data.ID - 1].transform.GetChild(1).gameObject.GetComponent<Dropdown>().value = msg.data.cartype;
		myData[msg.data.ID - 1]  = msg.data;
		bug1 = false;
	}

	public void PlayerDropdownChange(Dropdown dd){
		if(bug1){
			return;
		}
		myData[myID].cartype = dd.value;
		UpdatePlayerData msg = new UpdatePlayerData();
		msg.data = myData[myID];
		nc.Send(UpdatePlayerMsg, msg);
	}
}
