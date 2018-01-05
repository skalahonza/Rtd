using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpLobbyProxy : MonoBehaviour {

	public LobbyController lc;

	public void Host(){
		lc.Host();
	}

	public void Connect(){
		lc.Connect();
	}
}
