using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class Player : NetworkBehaviour {
    public override void OnStartLocalPlayer()
     {
        //GameObject.Find("Camera").GetComponent(SmoothFollow).target = transform;
     }
}
     
