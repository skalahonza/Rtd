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

public class MessageData : MessageBase {
	public string msg;
	public int id;
}

public class KickData : MessageBase {
	public int id;
}

public class PlayerDisconnectData : MessageBase {
	public int id;
}

public class PlayerReadyStateChangeData : MessageBase {
	public int id;
	public bool ready;
}

[RequireComponent(typeof(Lobby))]
public class LobbyController : NetworkBehaviour {
	public GameObject spawn;
    public GameObject spawnObject;	
	public GameObject hostSpawnObject;	
	public Text chatText;
	public SpConfig[] maps;
	public int mapIndex = 0;
	public string cname = "Player";
    public _car[] cars;
	public GameObject[] playerSetupObj = new GameObject[5];
	public Color[] colors = new Color[5];

	bool bug1 = false;
	bool bug712042 = false;
	public Lobby lobby;
	short SetMapMsg = 1024;
	short UpdatePlayerMsg = 1025;
	short AddPlayerMsg = 1027;
	short InstantiateMsg = 1026;
	short SendMessageMsg = 1028;
	short KickPlayerMsg = 1029;
    short PlayerDisconnectMsg = 1030;

	NetworkClient nc;
	public int myID;
	int msgstotal = 0;
	public LobbyPlayerData[] myData = new LobbyPlayerData[5];

	public void Start(){
		playerSetupObj = new GameObject[5];
		playerSetupObj = new GameObject[5];
		spawn = GameObject.Find("player_setup");
		gameObject.SetActive(true);
		lobby = GetComponent<Lobby>();
		DontDestroyOnLoad(gameObject);
	}

	public void Host () {
		bug712042 = true;
		spawnObject = hostSpawnObject;
		nc = lobby.StartHost();
		InitializeNetworkClient(nc);
		cname = GameObject.Find("plrname").GetComponent<Text>().text;
		GameObject.Find("ConnectForm").SetActive(false);
	}
	
	public void Connect () {
		int port = 7777;
		cname = GameObject.Find("plrname").GetComponent<Text>().text;
		Int32.TryParse(GameObject.Find("port").GetComponent<Text>().text, out port );
		lobby.networkPort = port;
		lobby.networkAddress = GameObject.Find("addr").GetComponent<Text>().text;
		nc = lobby.StartClient(); 
		InitializeNetworkClient(nc);
		GameObject.Find("ConnectForm").SetActive(false);
	}

	void InitializeNetworkClient(NetworkClient nc){
		GameObject.Find("ConnectForm").GetComponent<NetwokResolution>().Open();
		nc.RegisterHandler(SetMapMsg, SetMap);
		nc.RegisterHandler(AddPlayerMsg, AddPlayer);
		nc.RegisterHandler(UpdatePlayerMsg, UpdatePlayer);
		nc.RegisterHandler(InstantiateMsg, OnConnected);
		nc.RegisterHandler(SendMessageMsg, ReceiveMessage);
		nc.RegisterHandler(KickPlayerMsg, Kicked);
		nc.RegisterHandler(PlayerDisconnectMsg, Disconnected);
		spawn = GameObject.Find("player_setup");
	}

	public void Disconnected(NetworkMessage netMsg){
        var msg = netMsg.ReadMessage<PlayerDisconnectData>();
		//remove line and data
		myData[msg.id] = null;
		Destroy(playerSetupObj[msg.id]);
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
		Debug.Log(string.Format("mapreset to {0}", mapIndex));
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
		go.transform.GetChild(0).gameObject.GetComponent<Text>().color = colors[msg.data.material]; 
		if(myData[myID].ID != msg.data.ID){
			go.transform.GetChild(1).gameObject.GetComponent<Dropdown>().interactable = false;
		}
		playerSetupObj[msg.data.ID ] = go;
		myData[msg.data.ID]  = msg.data;
	}

	public void OnConnected(NetworkMessage netMsg){
		InstantiateData inst = netMsg.ReadMessage<InstantiateData>();
		mapIndex = inst.mapIndex;
		ResetMap();
		AddPlayerData msg = new AddPlayerData();
		msg.data = new LobbyPlayerData();
		myID = msg.data.ID = inst.ID;
		msg.data.material = inst.ID;
		msg.data.cname = cname;
		myData[myID] = msg.data;
		msg.data.cartype = 0;
		nc.Send(AddPlayerMsg, msg);
		foreach(var plr in inst.players){
			GameObject go = Instantiate(spawnObject, spawn.transform).gameObject;
        	go.transform.GetChild(0).gameObject.GetComponent<Text>().text = plr.cname;
			go.transform.GetChild(0).gameObject.GetComponent<Text>().color = colors[plr.material]; 
			go.transform.GetChild(1).gameObject.GetComponent<Dropdown>().interactable = false;
			bug1 = true;
     		go.transform.GetChild(1).gameObject.GetComponent<Dropdown>().value = plr.cartype;
			bug1 = false;
			playerSetupObj[plr.ID] = go;
			myData[plr.ID] = plr;
		}
    }

	public void UpdatePlayer(NetworkMessage netMsg){
		var msg = netMsg.ReadMessage<UpdatePlayerData>();
		bug1 = true;
		playerSetupObj[msg.data.ID].transform.GetChild(1).gameObject.GetComponent<Dropdown>().value = msg.data.cartype;
		playerSetupObj[msg.data.ID].transform.GetChild(0).gameObject.GetComponent<Text>().color = colors[msg.data.material]; 
		myData[msg.data.ID]  = msg.data;
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

	public void SendMessage(){
		MessageData msg = new MessageData();
		msg.msg = GameObject.Find("InputField").GetComponent<InputField>().text;
		if(msg.msg == "")
			return;
		msg.id = myID;
		nc.Send(SendMessageMsg, msg);
		GameObject.Find("InputField").GetComponent<InputField>().text = "";
	}

	public void ReceiveMessage(NetworkMessage netMsg){
		msgstotal++;
		var msg = netMsg.ReadMessage<MessageData>();
		Text tst = Instantiate(chatText, GameObject.Find("ChatWin").transform);
		tst.text = "<color=#"+ColorUtility.ToHtmlStringRGBA( colors[myData[msg.id].material] )+">" + myData[msg.id].cname + "</color> : " + msg.msg;
		//Debug.Log(string.Format("XAPI {0}",GameObject.Find("chatscroll").GetComponent<ScrollRect>().content.anchorMin));
		GameObject.Find("chatscroll").GetComponent<ScrollRect>().content.sizeDelta = new Vector2(0,msgstotal*21); 
		GameObject.Find("chatscroll").GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
	}

	public void Kick(Button btt){
		GameObject par = btt.transform.parent.gameObject;
		KickData msg = new KickData();
		int id = 0;
		foreach(var pld in playerSetupObj){
			if(pld == par){
				break;
			}
			id++;
		}
		msg.id = id;
		nc.Send(KickPlayerMsg, msg);
	}

	public void Kicked(NetworkMessage netMsg){
		Back();
	}

	public void Back(){
		Debug.Log("disconnect");
		nc.Disconnect();
		nc.Shutdown();
		if(bug712042){
			lobby.StopHost();
		}else{
			lobby.StopClient();
		}
		Destroy(GameObject.Find("GameObject"));
		SceneManager.LoadScene("Menu");
	}
}
