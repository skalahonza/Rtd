using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Car;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;
using Assets.Scripts.Constants;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CarControl))]
public class NetworkPlayer : NetworkBehaviour {


    CarControl cw;
    LocalPlayer cc ;
    public int material;
    public int ctype;

    void Start() 
    {
        if(isLocalPlayer) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }
        Debug.Log(string.Format("material for car {0} {1}", material, ctype));
        Material mat = GameObject.Find("network").GetComponent<LobbyController>().cars[ctype].materials[material];
        transform.GetChild(0).GetComponent<Renderer>().material = mat;
        transform.GetChild(1).GetComponent<Renderer>().material = mat;
        transform.GetChild(2).GetComponent<Renderer>().material = mat;
        transform.GetChild(3).GetComponent<Renderer>().material = mat;
        transform.GetChild(4).GetComponent<Renderer>().material = mat;  
    }

    public void startRace(){
        cc.StartRace(GameObject.Find("metadata").GetComponent<Map>());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(mode == LoadSceneMode.Additive){
            cc = gameObject.AddComponent<LocalPlayer>();
            cc.startRace = true; 
            Counter counter = GameObject.FindObjectOfType<Counter>();
            counter.setDelegate(startRace);
            return;
        }

    }
}