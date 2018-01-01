using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Singleplayer : MonoBehaviour {

	public Dropdown[] dropdown;
	public Dropdown[] cartypes;
	public Dropdown latestplayer;
	public Button nextmap;
	public Button prevmap;
	public Button play;
	public SpConfig[] maps;
	public int mapIndex = 0;
    public _car[] cars;

	public void select(Dropdown dd){
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
			int i = 0;
			foreach (var item in dropdown)
			{
				if(item.value == 0){
					game.addPlayer( "LocalPlayer", cars[cartypes[i].value].car, cars[cartypes[i].value].materials[i]);
				}else if(item.value == 1){
					game.addPlayer( "AIPlayer", cars[cartypes[i].value].car, cars[cartypes[i].value].materials[i]);
				}
				i++;
			}
            DontDestroyOnLoad(game);
            SceneManager.LoadScene(maps[mapIndex].scene);
            game.enabled = false;
	}
}