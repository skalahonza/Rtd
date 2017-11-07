using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;
using Assets.Scripts.Constants;
[RequireComponent(typeof(ControlWheel))]
public class Player : NetworkBehaviour {


    ControlWheel cw;
    
    void Start() 
    {
        cw = gameObject.GetComponent<ControlWheel>();
    }

    void FixedUpdate()
    {
        if (isLocalPlayer && cw != null)
        {
            cw.Move();
        } 
    }
}