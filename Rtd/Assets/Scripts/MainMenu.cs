using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

   
public class MainMenu : MonoBehaviour {

    public bool isSingle;
    public bool isMulti;
    public bool isQuit;
    public _car[] cars;
	void OnMouseUp() {
        GetComponent<Renderer>().material.color = Color.cyan;

        if (isSingle)
        {
            //Slámo pořež to
            Game game = GameObject.FindObjectOfType<Game>();
            game.addPlayer( "LocalPlayer", cars[0].car, cars[0].materials[0]);
            game.addPlayer( "AIPlayer", cars[0].car, cars[0].materials[1]);
            game.addPlayer( "AIPlayer", cars[0].car, cars[0].materials[2]);
            //game.addPlayer( "AIPlayer", cars[0].car, cars[0].materials[3]);
            //game.addPlayer( "AIPlayer", cars[0].car, cars[0].materials[4]);
            DontDestroyOnLoad(game);
            SceneManager.LoadScene(1);
            game.enabled = false;
        }
        else if (isMulti) {
            SceneManager.LoadScene(0);
        }
        else if (isQuit) {
            Application.Quit();
        }
    }

}
