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
	public string plrname;
}

public class KickData : MessageBase {
	public int id;
}

[RequireComponent(typeof(Lobby))]
public class LobbyController : NetworkBehaviour {
	public GameObject spawn;
	public GameObject projectileSpawner;
    public GameObject spawnObject;	
	public GameObject hostSpawnObject;	
	public GameObject mmaker;
	public Text chatText;
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
	short SendMessageMsg = 1028;
	short KickPlayerMsg = 1029;

	NetworkClient nc;
	int myID;
	int msgstotal = 0;
	public LobbyPlayerData[] myData = new LobbyPlayerData[5];

	public void Start(){
		gameObject.SetActive(true);
		lobby = GetComponent<Lobby>();
		mmaker.SetActive(false);
		projectileSpawner.SetActive(true);
		DontDestroyOnLoad(projectileSpawner);
	}

	public void Host () {
		spawnObject = hostSpawnObject;
		nc = lobby.StartHost();
		InitializeNetworkClient(nc);
		cname = GameObject.Find("plrname").GetComponent<Text>().text;
		GameObject.Find("ConnectForm").SetActive(false);
		mmaker.SetActive(true);
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
		mmaker.SetActive(true);
	}

	void InitializeNetworkClient(NetworkClient nc){
		nc.RegisterHandler(SetMapMsg, SetMap);
		nc.RegisterHandler(AddPlayerMsg, AddPlayer);
		nc.RegisterHandler(UpdatePlayerMsg, UpdatePlayer);
		nc.RegisterHandler(InstantiateMsg, OnConnected);
		nc.RegisterHandler(SendMessageMsg, ReceiveMessage);
		nc.RegisterHandler(KickPlayerMsg, Kicked);
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

	public void Back(){
		SceneManager.LoadScene("Menu");
	}

	public void SendMessage(){
		MessageData msg = new MessageData();
		msg.msg = GameObject.Find("InputField").GetComponent<InputField>().text;
		if(msg.msg == "")
			return;
		msg.plrname = myData[myID].cname;
		nc.Send(SendMessageMsg, msg);
		GameObject.Find("InputField").GetComponent<InputField>().text = "";
	}

	public void ReceiveMessage(NetworkMessage netMsg){
		msgstotal++;
		var msg = netMsg.ReadMessage<MessageData>();
		Text tst = Instantiate(chatText, GameObject.Find("ChatWin").transform);
		tst.text = msg.plrname + " : " + msg.msg;
		//Debug.Log(string.Format("XAPI {0}",GameObject.Find("chatscroll").GetComponent<ScrollRect>().content.anchorMin));
		GameObject.Find("chatscroll").GetComponent<ScrollRect>().content.sizeDelta = new Vector2(0,msgstotal*21); 
		GameObject.Find("chatscroll").GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
	}

	public void Kick(Button btt){
		KickData msg = new KickData();
		msg.id = 0 ;
		nc.Send(KickPlayerMsg, msg);
	}

	public void Kicked(NetworkMessage netMsg){

		//destroy all players
		//destroy  network
		Destroy(GameObject.Find("network"));
		Destroy(GameObject.Find("GameObject"));
		NetworkManager.Shutdown();
		Application.LoadLevel("Menu");
	}
}
