using UnityEngine;
using Assets.Scripts.Car;

public abstract class Player : MonoBehaviour
{
    public Checkpoint latest;
    public bool startRace = false;
    protected Map map;
    public int checkpointOffest = 0;


    /// <summary>
    /// Respawns the car on the latest checpoint position
    /// </summary>
    /// <param name="restore">True if the stats of the car shall be restored</param>
    public void Respawn(bool restore = true)
    {
        CarSpirit spirit = GetComponent<CarSpirit>();
        CarControl control = GetComponent<CarControl>();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        control.setUpdate(0, Vector3.zero);
        gameObject.transform.rotation = latest.positions[0].transform.rotation;

        if (restore)
        {
            spirit.Hp = spirit.MaxHp;
        }
    }

    public void StartRace(Map map)
    {
        latest = map.checkpoints[0];
        this.map = map;
        startRace = true;
    }
}