using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Leaderboards))]
public class Game : MonoBehaviour {

    List<string> players = new List<string>();
    List<GameObject> prefabs = new List<GameObject>();
    List<Material> materials = new List<Material>();
    public List<GameObject> cars = new List<GameObject>();
    public Leaderboards leaderboards ;
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

    public void finish(){
        Destroy(gameObject);
        Destroy(this);

        SceneManager.LoadScene("Menu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name == "Menu" || scene.name == "Singleplayer" || scene.name == "Multiplayer")
            return;
        if(mode == LoadSceneMode.Additive){
            foreach(var car in cars){
                car.SetActive(true);
            }   
            Counter counter = GameObject.FindObjectOfType<Counter>();
            counter.setDelegate(startRace);
            HUD hud  = GameObject.FindObjectOfType<HUD>();
            hud.setDelegate(finish);
            return;
        }
        map = GameObject.FindObjectOfType<Map>();
        //instantiate them all
        int i = 0;
        //get Map
        leaderboards = map.leaderboards;
        foreach (var item in prefabs)
        {
            cars.Add(Instantiate(item)); 
            Destroy (cars[i].GetComponent<NetworkPlayer>());
            cars[i].transform.position = map.checkpoints[0].positions[i].transform.position;
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
        foreach(var driver in players)
        {
            Player x = (Player) cars[i++].AddComponent(GetTypeFromName(driver));
            x.cname =  GetNameFromName(driver);
            x.cid = i-1;
        }
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }

    private Type GetTypeFromName(string typeName)
    {
        if (typeof(LocalPlayer).Name.Contains(typeName))
        {
            return typeof(LocalPlayer);
        }
        if (typeof(AIPlayer).Name.Contains(typeName))
        {
            return typeof(AIPlayer);
        }
        else
        {
            throw new TypeLoadException("No such type of player exists: " + typeName);
        }
    }
      private string GetNameFromName(string typeName){
          if (typeof(LocalPlayer).Name.Contains(typeName))
        {
            return "Player";
        }
        if (typeof(AIPlayer).Name.Contains(typeName))
        {
            return "AI";
        }
        else
        {
            return "unknown";
        }
      }
}