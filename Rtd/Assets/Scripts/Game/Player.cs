using UnityEngine;

public abstract class Player : MonoBehaviour {
    public Checkpoint latest;
    public bool startRace = false;
    protected Map map;
    public int checkpointOffest = 0;

    

    void respawn(){
        
    }

    public void StartRace(Map map){
        latest = map.checkpoints[0];
        this.map = map;
        startRace = true;
    }
}