using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Singleplayer : MonoBehaviour {

	public Dropdown[] dropdown;
	public Dropdown latestplayer;
	public Button nextmap;
	public Button prevmap;
	public Button play;
	public SpConfig[] maps;
	public int mapIndex = 0;
    public _car[] cars;

	public void select(Dropdown dd){
		Debug.Log(string.Format("value {0}", dd.value));
		if(dd.value == 0){
			latestplayer.value = 1;
			latestplayer = dd;		
		}
	}

	public void nextMap(){
		if(maps.Length == mapIndex+1){
			return;
		}
		maps[mapIndex].image.enabled = false;
		mapIndex++;
		maps[mapIndex].image.enabled = true;
	}

	public void prevMap(){
		if(0 == mapIndex){
			return;
		}
		maps[mapIndex].image.enabled = false;
		mapIndex--;
		maps[mapIndex].image.enabled = true;
	}

	public void startGame(){
		    Game game = GameObject.FindObjectOfType<Game>();
			foreach (var item in dropdown)
			{
				if(item.value == 0){
					game.addPlayer( "LocalPlayer", cars[0].car, cars[0].materials[0]);
				}else if(item.value == 1){
					game.addPlayer( "AIPlayer", cars[0].car, cars[0].materials[1]);
				}
			}
            DontDestroyOnLoad(game);
            SceneManager.LoadScene(maps[mapIndex].scene.buildIndex);
            game.enabled = false;
	}
	
}
