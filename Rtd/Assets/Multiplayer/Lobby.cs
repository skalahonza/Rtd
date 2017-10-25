using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Lobby : NetworkLobbyManager {

void OnLobbyStartHost(){
networkAddress = "0.0.0.0";
}
}
