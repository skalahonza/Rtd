using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Linq;


[RequireComponent(typeof(Lobby))]
public class NetClient : NetworkClient {

    public void OnConnected(NetworkConnection conn, NetworkReader reader)
    {
        Debug.Log("Connected to server");
    }

    public void OnDisconnected(NetworkConnection conn, NetworkReader reader)
    {
        Debug.Log("Disconnected from server");
    }

    public void OnError(NetworkConnection conn, NetworkReader reader)
    {
        //SystemErrorMessage errorMsg = reader.SmartRead<SystemErrorMessage>();
        Debug.Log("Error connecting with code ");
    }
	// Use this for initialization
	public void Host () {
		//GetComponent<NetworkManager>().StartHost();
	}
	
	// Update is called once per frame
	public void Connect () {
		int port = 8080;
		Int32.TryParse(GameObject.Find("Port").GetComponent<Text>().text, out port );
		Connect(GameObject.Find("Address").GetComponent<Text>().text, port);
	}
}
