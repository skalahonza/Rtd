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

    void Start() 
    {
        cw = gameObject.GetComponent<CarControl>();
        if (!isLocalPlayer && cw != null)
        {
            Destroy(cw);
        }else if(isLocalPlayer) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }
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