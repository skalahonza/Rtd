using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.blue;
    }
	
}
