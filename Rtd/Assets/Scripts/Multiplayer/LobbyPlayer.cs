using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LobbyPlayer : NetworkLobbyPlayer {

	LobbyPlayerData data = new LobbyPlayerData();

	public void Play(){
		SendReadyToBeginMessage();
	}
}
