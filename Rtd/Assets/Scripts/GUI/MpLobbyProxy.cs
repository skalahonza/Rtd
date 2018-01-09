using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// proxy for handling multiplayer lobby GUI
/// </summary>
public class MpLobbyProxy : MonoBehaviour {

	public InputField msg;
	public LobbyController lc;

	/// <summary>
	/// click on Host button
	/// </summary>
	public void Host () {
		lc.Host ();
	}

	/// <summary>
	/// click on connect button
	/// </summary>
	public void Connect () {
		lc.Connect ();
	}

	/// <summary>
	/// click on Back button - when connected or hosting game
	/// </summary>
	public void Back () {
		lc.Back ();
	}

	/// <summary>
	/// click on next ma button
	/// </summary>
	public void nextMap () {
		lc.nextMap ();
	}

	/// <summary>
	/// click on prev map button
	/// </summary>
	public void prevMap () {
		lc.prevMap ();
	}

	/// <summary>
	/// send chat message
	/// </summary>
	public void Send () {
		lc.SendMessage ();
	}

	/// <summary>
	/// back from multiplayer - when not connected
	/// </summary>
	public void Bback () {
		Destroy (GameObject.Find ("GameObject"));
		SceneManager.LoadScene ("Menu");
	}

}