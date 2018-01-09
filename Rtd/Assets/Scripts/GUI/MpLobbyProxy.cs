using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MpLobbyProxy : MonoBehaviour {

	public InputField msg;
	public LobbyController lc;

	public void Host(){
		lc.Host();
	}

	public void Connect(){
		lc.Connect();
	}

	public void Back(){
		lc.Back();
	}

	public void nextMap(){
		lc.nextMap();
	}

	public void prevMap(){
		lc.prevMap();
	}

	public void Send(){
		lc.SendMessage();
	}

	public void Bback(){
		Destroy (GameObject.Find ("GameObject"));
		SceneManager.LoadScene ("Menu");
	}

}
