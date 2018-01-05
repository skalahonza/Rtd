using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LobbyPlayer : NetworkLobbyPlayer {

	[SyncVar]
	int ID;

	public void Start(){
		if(isLocalPlayer){
			LobbyController lc = GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>();
			ID = lc.myID;
			GameObject.Find("Play").GetComponent<Button>().onClick.AddListener(Play);
		}
	
	}

	public void Play(){
		Button play = GameObject.Find("Play").GetComponent<Button>();
		if(play.GetComponentInChildren<Text>().text == "Ready"){
			play.GetComponentInChildren<Text>().text = "Not Ready";
			SendReadyToBeginMessage();
		}else{
			play.GetComponentInChildren<Text>().text = "Ready";
			SendNotReadyToBeginMessage();
		}
	}

	public override void OnClientReady(bool readyState){
		LobbyController lc = GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>();
		Debug.Log(string.Format("My ID: {0}", ID));
		GameObject setup = lc.playerSetupObj[ID];
		setup.transform.GetChild(0).gameObject.GetComponent<Text>().fontStyle = readyState ?  FontStyle.BoldAndItalic : FontStyle.Normal;
	}

	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}