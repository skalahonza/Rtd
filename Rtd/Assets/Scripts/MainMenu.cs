using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public bool isSingle;
    public bool isMulti;
    public bool isQuit;

	void OnMouseUp() {
        GetComponent<Renderer>().material.color = Color.yellow;

        if (isSingle)
        {
            SceneManager.LoadScene(1);
        }
        else if (isMulti) {
            SceneManager.LoadScene(2);
        }
        else if (isQuit) {
            Application.Quit();
        }
    }

}
