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
            LocalPlayer cc = gameObject.AddComponent<LocalPlayer>();
            cc.startRace = true; 
        }
    }

    void FixedUpdate()
    {

    }
}