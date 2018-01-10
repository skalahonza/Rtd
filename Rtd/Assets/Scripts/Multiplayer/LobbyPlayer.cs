using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Lobby player - hidden player object
/// </summary>
public class LobbyPlayer : NetworkLobbyPlayer {

	[SyncVar]
	public int ID;

	/// <summary>
	/// find ready button and set handler
	/// </summary>
	public void Start () {
		if (isLocalPlayer) {
			GameObject.Find ("Play").GetComponent<Button> ().onClick.AddListener (Play);
		}

	}

	/// <summary>
	/// read button handler
	/// </summary>
	public void Play () {
		Button play = GameObject.Find ("Play").GetComponent<Button> ();
		if (play.GetComponentInChildren<Text> ().text == "Ready") {
			play.GetComponentInChildren<Text> ().text = "Not Ready";
			SendReadyToBeginMessage ();
		} else {
			play.GetComponentInChildren<Text> ().text = "Ready";
			SendNotReadyToBeginMessage ();
		}
	}

	/// <summary>
	/// handle clients when they become ready
	/// </summary>
	/// <param name="readyState"></param>
	public override void OnClientReady (bool readyState) {
		LobbyController lc = GameObject.FindGameObjectsWithTag ("network") [0].GetComponent<LobbyController> ();
		GameObject setup = lc.playerSetupObj[ID];
		setup.transform.GetChild (0).gameObject.GetComponent<Text> ().fontStyle = readyState ? FontStyle.BoldAndItalic : FontStyle.Normal;
	}

	/// <summary>
	/// keep this object into game scene
	/// </summary>
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
}