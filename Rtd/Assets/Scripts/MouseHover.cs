using UnityEngine;

public class MouseHover : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.white;
    }
	
	// Update is called once per frame
	void OnMouseEnter () {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    private void OnMouseExit() {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
