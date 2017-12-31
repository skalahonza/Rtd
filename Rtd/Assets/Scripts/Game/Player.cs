using UnityEngine;
using Assets.Scripts.Car;

public abstract class Player : MonoBehaviour {
    public Checkpoint latest;
    public bool startRace = false;
    protected Map map;
    public int checkpointOffest = 0;

    

    public void respawn(){
        CarSpirit spirit = GetComponent<CarSpirit>();
        spirit.Hp = spirit.MaxHp;
        CarControl control = GetComponent<CarControl>();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        control.setUpdate(0, Vector3.zero);
        gameObject.transform.rotation = latest.positions[0].transform.rotation;
    }

    public void StartRace(Map map){
        latest = map.checkpoints[0];
        this.map = map;
        startRace = true;
    }
}