using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Leaderboards))]
public class Game : MonoBehaviour {

    List<string> players = new List<string>();
    List<GameObject> prefabs = new List<GameObject>();
    List<Material> materials = new List<Material>();
    List<GameObject> cars = new List<GameObject>();
    Map map;


    public void addPlayer(string player, GameObject carPrefab ,Material material){
        if(players.Count == 5)
            return;
        players.Add(player);
        prefabs.Add(carPrefab);
        materials.Add(material);
    }    
    
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void startRace(){
        foreach(var car in cars){
            car.GetComponent<Player>().StartRace(map);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        map = GameObject.FindObjectOfType<Map>();
        //instantiate them all
        int i = 0;
        //get Map
        foreach (var item in prefabs)
        {
            cars.Add(Instantiate(item)); 
            cars[i].transform.position = map.checkpoints[0].positions[i++].transform.position;
            cars[i].transform.rotation = map.checkpoints[0].positions[i++].transform.rotation;
        }
        i = 0;
        foreach (var material in materials){
            cars[i].transform.GetChild(0).GetComponent<Renderer>().material = material;
            cars[i].transform.GetChild(1).GetComponent<Renderer>().material = material;
            cars[i].transform.GetChild(2).GetComponent<Renderer>().material = material;
            cars[i].transform.GetChild(3).GetComponent<Renderer>().material = material;
            cars[i++].transform.GetChild(4).GetComponent<Renderer>().material = material;
        }
        i = 0;
        foreach(var driver in players){
            UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(cars[i++], "Assets/Scripts/Game/Game.cs (46,13)", driver); 
        }
        Counter counter = GameObject.FindObjectOfType<Counter>();
        counter.setDelegate(startRace);
    }
}