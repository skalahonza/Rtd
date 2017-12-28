using System.Collections;
using System.Collections.Generic;
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
            Camera.main.GetComponent<SmoothFollow>().target = this.transform;
        }
    }

    void FixedUpdate()
    {

    }
}