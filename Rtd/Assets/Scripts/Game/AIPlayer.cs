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
        //agent.updatePosition = false;
        //agent.updateRotation = false;
        agent.autoBraking = false;
        //agent.acceleration = spirit.MaxMotorTorque;
        agent.speed = spirit.MaxMotorTorque;
        agent.angularSpeed = spirit.MaxSteeringAngle;
        //set offset
        agent.baseOffset = 1.0f;
    }

    void FixedUpdate() {
        if(!startRace)
            return;
        if(map.checkpoints[checkpointOffest+1] != null)
            agent.destination = map.checkpoints[checkpointOffest+1].transform.position;
        //float angle = Vector3.Angle(agent.velocity.normalized, this.transform.forward);
        //angle = (angle + 180.0f) % 360.0f;
        //string output = "Velocity {0} Angle {1}";
        //Debug.Log(string.Format(output, agent.velocity.magnitude, angle));
        //control.setUpdate(1000, angle, 0.0f);
    }
}
