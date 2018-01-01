using Assets.Scripts.Car;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
class AIPlayer : Player{

    CarControl control;
    CarSpirit spirit;
    UnityEngine.AI.NavMeshAgent agent   ;
    void Start(){
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();

        //set info from car
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.autoBraking = false;
        //agent.acceleration = spirit.MaxMotorTorque;
        agent.speed = spirit.maxSpeed;
        agent.angularSpeed = 30;
        agent.height = 5.5f;
        agent.radius = 8.0f;
        //set offset
        agent.baseOffset = 1.2f;
    }

    void FixedUpdate() {
        if(!startRace)
            return;
        if(map.checkpoints[checkpointOffest+1] != null){
            agent.destination = map.checkpoints[checkpointOffest+1].transform.position;
        }
        float angle = Vector3.Angle(agent.velocity, this.transform.forward);
        control.setUpdate( spirit.MaxMotorTorque*agent.velocity.normalized.magnitude,angle, 0);
        if(!agent.isOnNavMesh){
            Respawn(false);
        }
    }
}
