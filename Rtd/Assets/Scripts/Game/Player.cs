using Assets.Scripts.Car;
using UnityEngine;

/// <summary>
/// Provides basec functions to all types of players
/// </summary>
[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public abstract class Player : MonoBehaviour {
    public Checkpoint latest;
    public bool startRace = false;
    protected Map map;
    public int checkpointOffest = 0;
    protected UnityEngine.AI.NavMeshAgent agent;
    float lastlen = 0.0f;
    public bool finished = false;
    public string cname = "playername";
    public int cid = 1;
    protected UnityEngine.AI.NavMeshPath path;
    protected int pathIndex = 0;

    /// <summary>
    /// Respawns the car on the latest checpoint position
    /// </summary>
    /// <param name="restore">True if the stats of the car shall be restored</param>
    public void Respawn (bool restore = true) {
        CarSpirit spirit = GetComponent<CarSpirit> ();
        CarControl control = GetComponent<CarControl> ();
        Rigidbody rigidbody = GetComponent<Rigidbody> ();
        rigidbody.velocity = Vector3.zero;
        control.setUpdate (0, Vector3.zero);
        gameObject.transform.position = latest.positions[0].transform.position;
        gameObject.transform.rotation = latest.positions[0].transform.rotation;

        // removep ossible flash respawn
        var cgr = spirit.gameObject.GetComponent<CarGhostRespawn> ();
        if (cgr != null)
            Destroy (cgr);

        //flash respawn
        spirit.gameObject.AddComponent<CarGhostRespawn> ();

        if (restore) {
            spirit.Hp = spirit.MaxHp;
        }
    }

    /// <summary>
    /// use this to calculate remaining distance
    /// </summary>
    private void CalculatePath () {
        if (latest != null) {
            path = agent.path;
            pathIndex = 0;
            agent.Warp (transform.position);
            agent.SetDestination (latest.positions[0].transform.position);
        }
    }
    
    /// <summary>
    /// This is called when race starts
    /// </summary>
    virtual protected void OnRaceStart () {

    }

    /// <summary>
    /// get remaining distance to the next checkpoint - calculate position
    /// </summary>
    /// <returns>
    /// remaining distance
    /// </returns>
    public float GetPathLength () {
        if (agent.pathPending || agent.remainingDistance == int.MaxValue) {
            return lastlen;
        }
        lastlen = agent.remainingDistance;
        CalculatePath ();
        return lastlen;
    }

    /// <summary>
    /// called on players when race starts
    /// </summary>
    /// <param name="map">
    /// Map information
    /// </param>
    public void StartRace (Map map) {
        latest = map.checkpoints[0];
        this.map = map;
        startRace = true;
        OnRaceStart ();
    }

    /// <summary>
    /// initialize when starting - set up navmesh agent
    /// </summary>
    public void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        GameStart ();
    }

    /// <summary>
    /// called in start method in player classes
    /// </summary>
    public virtual void GameStart () {

    }

    /// <summary>
    /// called when player finishes the track
    /// </summary>
    public void Finish () {
        finished = true;
    }
}