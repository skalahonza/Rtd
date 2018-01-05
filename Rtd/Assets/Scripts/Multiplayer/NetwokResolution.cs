using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//passing around bug 712042
public class NetwokResolution : MonoBehaviour {

	public GameObject spawn;
	public GameObject mmaker;
	// Use this for initialization
	void Start () {
		if(GameObject.Find("network") == null){
			GameObject inst = Instantiate(Resources.Load<GameObject>("Multiplayer/network"));
			LobbyController lc = GetComponent<MpLobbyProxy>().lc = inst.GetComponent<LobbyController>();
			lc.lobby = inst.GetComponent<Lobby>();
			lc.mmaker = mmaker;
			lc.spawn = spawn;
			inst.SetActive(true);
			lc.mmaker.SetActive(false);
		}
	}
}
