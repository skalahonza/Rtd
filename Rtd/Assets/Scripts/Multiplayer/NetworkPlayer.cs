using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Car;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;
using Assets.Scripts.Constants;

[RequireComponent(typeof(CarControl))]
public class NetworkPlayer : NetworkBehaviour {


    CarControl cw;
    
    void Start() 
    {
        cw = gameObject.GetComponent<CarControl>();
        if (!isLocalPlayer && cw != null)
        {
            Destroy(cw);
        }else if(isLocalPlayer) {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }
    }

    public void startRace(){
        foreach(var car in cars){
            car.GetComponent<Player>().StartRace(map);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(mode == LoadSceneMode.Additive){
            LocalPlayer cc = gameObject.AddComponent<LocalPlayer>();
            cc.startRace = true; 
            Counter counter = GameObject.FindObjectOfType<Counter>();
            counter.setDelegate(startRace);
            return;
        }

    }
}