using UnityEngine;
using Assets.Scripts.Car;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public abstract class Player : MonoBehaviour
{
    public Checkpoint latest;
    public bool startRace = false;
    protected Map map;
    public int checkpointOffest = 0;
    protected UnityEngine.AI.NavMeshAgent agent;
    float lastlen =0.0f;
    public bool finished = false;
    public string cname = "playername";
    public int cid = 1;

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
        gameObject.transform.position = latest.positions[0].transform.position;
        gameObject.transform.rotation = latest.positions[0].transform.rotation;

        // removep ossible flash respawn
        var cgr = spirit.gameObject.GetComponent<CarGhostRespawn>();
        if (cgr != null)
            Destroy(cgr);

        //flash respawn
        spirit.gameObject.AddComponent<CarGhostRespawn>();

        //agent.Warp (transform.position);
        if (restore)
        {
            spirit.Hp = spirit.MaxHp;
        }
    }

    private void CalculatePath( )
    {
        if(latest != null){
            agent.Warp(transform.position);
            agent.SetDestination(latest.positions[0].transform.position);
        }
    }
       
    public float GetPathLength( )
    {			
        if (agent.pathPending || agent.remainingDistance == int.MaxValue )
        {
            return lastlen;
        }
        lastlen = agent.remainingDistance;
        CalculatePath();
        return lastlen;
    }

    public void StartRace(Map map)
    {
        latest = map.checkpoints[0];
        this.map = map;
        startRace = true;
    }

    public void Start(){
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        agent.updatePosition = false;
		agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void Finish(){
        finished = true;
    }
}