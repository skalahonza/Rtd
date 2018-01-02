using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LobbyPlayer : NetworkLobbyPlayer {

	public void Play(){
		readyToBegin = true;
		SendReadyToBeginMessage();
	}
}
