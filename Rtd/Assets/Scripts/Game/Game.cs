using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// lass reprezenting game data - singleplayer only
/// </summary>
[RequireComponent (typeof (Leaderboards))]
public class Game : MonoBehaviour {

    List<string> players = new List<string> ();
    List<GameObject> prefabs = new List<GameObject> ();
    List<Material> materials = new List<Material> ();
    public List<GameObject> cars = new List<GameObject> ();
    public Leaderboards leaderboards;
    Map map;

    /// <summary>
    /// Add player to game
    /// </summary>
    /// <param name="player">
    /// name of class which will handle this player
    /// currently - AIPlayer or Localplayer - number of localplayer instances not checked please check this by yourself
    /// </param>
    /// <param name="carPrefab">
    /// car to be spawned
    /// </param>
    /// <param name="material">
    /// Material to that car with corresponding color
    /// </param>
    public void addPlayer (string player, GameObject carPrefab, Material material) {
        if (players.Count == 5)
            return;
        players.Add (player);
        prefabs.Add (carPrefab);
        materials.Add (material);
    }

    /// <summary>
    /// Handle module enabling
    /// </summary>
    private void OnEnable () {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /// <summary>
    /// Countdown finish wrapper
    /// </summary>
    public void startRace () {
        foreach (var car in cars) {
            car.GetComponent<Player> ().StartRace (map);
        }
    }

    /// <summary>
    /// finish game remove object
    /// </summary>
    public void finish () {
        Destroy (gameObject);
        SceneManager.LoadScene ("Menu");
    }
    /// <summary>
    /// dont forget to remove load scene delegate
    /// </summary>
    public void OnDestroy () {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// handle scene loading HUD scene + level scene
    /// </summary>
    /// <param name="scene">
    /// scene
    /// </param>
    /// <param name="mode">
    /// loading mode
    /// </param>
    private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
        if (mode == LoadSceneMode.Additive) {
            foreach (var car in cars) {
                car.SetActive (true);
            }
            Counter counter = GameObject.FindObjectOfType<Counter> ();
            counter.setDelegate (startRace);
            HUD hud = GameObject.FindObjectOfType<HUD> ();
            hud.setDelegate (finish);
            return;
        }
        map = GameObject.FindObjectOfType<Map> ();
        //instantiate them all
        int i = 0;
        //get Map
        leaderboards = map.leaderboards;
        foreach (var item in prefabs) {
            cars.Add (Instantiate (item));
            Destroy (cars[i].GetComponent<NetworkPlayer> ());
            cars[i].transform.position = map.checkpoints[0].positions[i].transform.position;
            cars[i].transform.rotation = map.checkpoints[0].positions[i++].transform.rotation;
        }
        i = 0;
        foreach (var material in materials) {
            cars[i].transform.GetChild (0).GetComponent<Renderer> ().material = material;
            cars[i].transform.GetChild (1).GetComponent<Renderer> ().material = material;
            cars[i].transform.GetChild (2).GetComponent<Renderer> ().material = material;
            cars[i].transform.GetChild (3).GetComponent<Renderer> ().material = material;
            cars[i++].transform.GetChild (4).GetComponent<Renderer> ().material = material;
        }
        i = 0;
        foreach (var driver in players) {
            Player x = (Player) cars[i++].AddComponent (GetTypeFromName (driver));
            x.cname = GetNameFromName (driver);
            x.cid = i - 1;
        }
        SceneManager.LoadScene ("HUD", LoadSceneMode.Additive);
    }
    
    /// <summary>
    /// Get player type from string
    /// </summary>
    /// <param name="typeName">
    /// string reprezentin type
    /// </param>
    /// <returns>player type class</returns>
    private Type GetTypeFromName (string typeName) {
        if (typeof (LocalPlayer).Name.Contains (typeName)) {
            return typeof (LocalPlayer);
        }
        if (typeof (AIPlayer).Name.Contains (typeName)) {
            return typeof (AIPlayer);
        } else {
            throw new TypeLoadException ("No such type of player exists: " + typeName);
        }
    }

    /// <summary>
    /// get name of player based on class - for Singlepayer
    /// </summary>
    /// <param name="typeName">
    /// player type
    /// </param>
    /// <returns>player name</returns>
    private string GetNameFromName (string typeName) {
        if (typeof (LocalPlayer).Name.Contains (typeName)) {
            return "Player";
        }
        if (typeof (AIPlayer).Name.Contains (typeName)) {
            return "AI";
        } else {
            return "unknown";
        }
    }
}