using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
class AIPlayer : Player{
    void FixedUpdate() {
        if(!startRace)
            return;
        UnityEngine.AI.NavMeshAgent agent =  GetComponent<UnityEngine.AI.NavMeshAgent>();
        if(map.checkpoints[checkpointOffest+1] != null)
            agent.destination = map.checkpoints[checkpointOffest+1].transform.position;
    }
}