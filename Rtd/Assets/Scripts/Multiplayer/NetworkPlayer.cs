using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Car;
using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Utility;

/// <summary>
/// network player game data
/// </summary>
[RequireComponent (typeof (CarControl))]
public class NetworkPlayer : NetworkBehaviour {

    CarControl cw;
    LocalPlayer cc;
    [SyncVar]
    public int pid;
    [SyncVar]
    public int cid;
    [SyncVar]
    public string cname;
    [SyncVar]
    public float dist;
    [SyncVar]
    public int checkpointOffest = 0;

    /// <summary>
    /// start game and setup materials
    /// </summary>
    void Start () {
        if (isLocalPlayer) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene ("HUD", LoadSceneMode.Additive);
        }
        LobbyController lc = GameObject.FindGameObjectsWithTag ("network") [0].GetComponent<LobbyController> ();
        LobbyPlayerData data = lc.myData[pid];
        Material mat = lc.cars[data.cartype].materials[data.material];
        transform.GetChild (0).GetComponent<Renderer> ().material = mat;
        transform.GetChild (1).GetComponent<Renderer> ().material = mat;
        transform.GetChild (2).GetComponent<Renderer> ().material = mat;
        transform.GetChild (3).GetComponent<Renderer> ().material = mat;
        transform.GetChild (4).GetComponent<Renderer> ().material = mat;
    }

    /// <summary>
    /// handle start race - call driver
    /// </summary>
    public void startRace () {
        cc.StartRace (GameObject.Find ("metadata").GetComponent<Map> ());
    }

    /// <summary>
    /// setup HUD data
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
        if (mode == LoadSceneMode.Additive) {
            cc = gameObject.AddComponent<LocalPlayer> ();
            cc.cid = cid;
            cc.cname = cname;
            Counter counter = GameObject.FindObjectOfType<Counter> ();
            counter.setDelegate (startRace);
            HUD hud = GameObject.FindObjectOfType<HUD> ();
            hud.setDelegate (FinishGame);
            hud.mynp = this;
            return;
        }
    }

    /// <summary>
    /// remove scene manager delegate
    /// </summary>
    public void OnDestroy () {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// handle finishing game
    /// </summary>
    public void FinishGame () {
        LobbyController lb = GameObject.FindObjectOfType<LobbyController> ();
        lb.Back ();
    }

    /// <summary>
    /// handle player track finishing 
    /// </summary>
    /// <param name="plid">
    /// player ID
    /// </param>
    /// <param name="cname">
    /// player name
    /// </param>
    [Command]
    public void CmdFinished (int plid, string cname) {
        RpcAddFinishedPlayer (plid, cname);
    }

    /// <summary>
    /// handle player track finishing 
    /// </summary>
    /// <param name="plid">
    /// player ID
    /// </param>
    /// <param name="cname">
    /// player name
    /// </param>
    [ClientRpc]
    private void RpcAddFinishedPlayer (int plid, string cname) {
        Leaderboards lb = GameObject.FindObjectOfType<Leaderboards> ();
        Playerx p = new Playerx ();
        p.cid = plid;
        p.cname = cname;
        lb.players.Add (p);
    }

    /// <summary>
    /// get remainging distance to next checkpoint
    /// </summary>
    /// <returns>
    /// remaining distance
    /// </returns>
    public float GetPathLength () {
        return dist;
    }

    /// <summary>
    /// synchronize remaining lengths
    /// </summary>
    public void Update () {
        if (isLocalPlayer && cc != null) {
            CmdSyncLen (cc.checkpointOffest, cc.GetPathLength ());
        }
    }

    /// <summary>
    /// synchronize remaining lengths
    /// </summary>
    /// <param name="chk">
    /// player checkpoint index
    /// </param>
    /// <param name="dest">
    /// remaining distance
    /// </param>
    [Command]
    public void CmdSyncLen (int chk, float dest) {
        RpcSyncLen (chk, dest);
    }

    /// <summary>
    /// synchronize remaining lengths
    /// </summary>
    /// <param name="chk">
    /// player checkpoint index
    /// </param>
    /// <param name="dest">
    /// remaining distance
    /// </param>
    [ClientRpc]
    private void RpcSyncLen (int chk, float dest) {
        checkpointOffest = chk;
        dist = dest;
    }
}