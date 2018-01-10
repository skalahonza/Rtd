using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// singleplayer lobby GUI handler
/// </summary>
public class Singleplayer : MonoBehaviour {

	public Dropdown[] dropdown;
	public Dropdown[] cartypes;
	public Dropdown latestplayer;
	public Button nextmap;
	public Button prevmap;
	public Button play;
	public SpConfig[] maps;
	public int mapIndex;
	public _car[] cars;

	/// <summary>
	/// select car type
	/// </summary>
	/// <param name="dd">
	/// current dropdown
	/// </param>
	public void select (Dropdown dd) {
		if (dd.value == 0) {
			latestplayer.value = 1;
			latestplayer = dd;
		}
	}

	/// <summary>
	/// click on next map button
	/// </summary>
	public void nextMap () {
		if (maps.Length == mapIndex + 1) {
			return;
		}
		maps[mapIndex].image.enabled = false;
		mapIndex++;
		maps[mapIndex].image.enabled = true;
	}

	/// <summary>
	/// click on prev ma button
	/// </summary>
	public void prevMap () {
		if (0 == mapIndex) {
			return;
		}
		maps[mapIndex].image.enabled = false;
		mapIndex--;
		maps[mapIndex].image.enabled = true;
	}

	/// <summary>
	/// click on start game button 
	/// </summary>
	public void startGame () {
		Game game = FindObjectOfType<Game> ();
		game.enabled = true;
		int i = 0;
		foreach (var item in dropdown) {
			if (item.value == 0) {
				game.addPlayer ("LocalPlayer", cars[cartypes[i].value].car, cars[cartypes[i].value].materials[i]);
			} else if (item.value == 1) {
				game.addPlayer ("AIPlayer", cars[cartypes[i].value].car, cars[cartypes[i].value].materials[i]);
			}
			i++;
		}
		DontDestroyOnLoad (game);
		SceneManager.LoadScene (maps[mapIndex].scene);
		game.enabled = false;
	}

	/// <summary>
	/// click on back button
	/// </summary>
	public void Back () {
		SceneManager.LoadScene ("Menu");
	}
}