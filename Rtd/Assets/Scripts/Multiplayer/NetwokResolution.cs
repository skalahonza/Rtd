﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//passing around bug 712042
public class NetwokResolution : MonoBehaviour {

	public GameObject mmaker;
	// Use this for initialization
	void Start () {
		if(GameObject.FindGameObjectsWithTag("network").Length == 0){
			GameObject inst = Instantiate(Resources.Load<GameObject>("Multiplayer/network"));
			LobbyController lc = GetComponent<MpLobbyProxy>().lc = inst.GetComponent<LobbyController>();
			lc.lobby = inst.GetComponent<Lobby>();
			inst.tag = "network";
			inst.SetActive(true);
		}
		mmaker.SetActive(false);
		GetComponent<MpLobbyProxy>().lc = GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>();
	}

	public void Open(){
		mmaker.SetActive(true);
	}
}
