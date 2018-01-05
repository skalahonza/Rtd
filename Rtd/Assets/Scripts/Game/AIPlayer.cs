using Assets.Scripts.Car;
using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
class AIPlayer : Player{
/*
    CarControl control;
	List<Transform> path;
	private int pathIndex ;
	float distFromPath = 20.0f;

	float maxSteer  = 20.0f;
	public float maxTorque  = 120.0f;
	float currentSpeed  = 0.0f;
	float topSpeed  = 150.0f;

	private bool isBreaking = false;
	public float breakForce = 500.0f;

	private bool isRunning  = true;
	float timer = 1.5f;
	float resetTimer = 500.0f;
	float m_Downforce = 100.0f;
    CarSpirit spirit;
    UnityEngine.AI.NavMeshAgent agent;

    void Start(){
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.autoBraking = false;
		GetPath ();
    }

    void FixedUpdate() {
        if(!startRace)
            return;
	    GetSteer ();
	    Move ();
	    Breaking ();	
        if(!agent.isOnNavMesh){
            Respawn(false);
        }
    }

    void GetSteer () {

	Vector3 steerVector  = transform.InverseTransformPoint( new Vector3(
		path[pathIndex].position.x,
		transform.position.y,
		path[pathIndex].position.z)
		);
	float newSteer  = maxSteer * (steerVector.x / steerVector.magnitude);
	
	wheelsCollider [0].steerAngle = newSteer;
	wheelsCollider [1].steerAngle = newSteer;	
	
	if (steerVector.magnitude <= distFromPath)
	{	
		pathIndex++;
		if (pathIndex >= path.Count)
		{
			pathIndex = 0;			
			GetPath ();
		}		
	}	
		
	if (Math.Abs (newSteer) >= 8 && GetComponent<Rigidbody>().velocity.magnitude >= 15)
	{					
		if (distFromPath != 0.0f)
		{
			isBreaking = true;
		}
	}
	else
	{
		isBreaking = false;
	}																																																																							
}

void GetPath () {	
        if(map.checkpoints.Length != checkpointOffest+1){
            agent.destination = map.checkpoints[checkpointOffest+1].transform.position;
        }
	//TODO: from agent
}

void Move () {
	
	if (isRunning)
	{		
		for (int i= 0; i < 4; i++)
		{	
			currentSpeed = 2*22 /7 * wheelsCollider[i].radius * wheelsCollider[i].rpm * 60/1000;
			currentSpeed = Mathf.Round(currentSpeed);
						
			if (currentSpeed < topSpeed && !isBreaking)
			{
				//accelarate the car
				float accelarate  = 1.0f;
				wheelsCollider[i].motorTorque = accelarate * maxTorque;
			}
			else
			{			
				wheelsCollider[i].motorTorque = 0;
			}
			AddDownForce();	
		}
	}
	else if (isRunning == false)
	{
		driveBackwards();
	}	
}

private void AddDownForce()
{
	//wheelsCollider[2].attachedRigidbody.AddForce(-transform.up*m_Downforce * wheelsCollider[3].attachedRigidbody.velocity.magnitude);
	this.GetComponent<Rigidbody>().AddRelativeForce(-transform.up * m_Downforce * GetComponent<Rigidbody>().velocity.magnitude);
}

void Breaking () {

	if (isBreaking)
	{			
		wheelsCollider[0].brakeTorque = breakForce;	
		wheelsCollider[1].brakeTorque = breakForce;	
		wheelsCollider[2].brakeTorque = breakForce;	
		wheelsCollider[3].brakeTorque = breakForce;
		
		wheelsCollider[0].motorTorque = 0.0f;	
		wheelsCollider[1].motorTorque = 0.0f;	
		wheelsCollider[2].motorTorque = 0.0f;	
		wheelsCollider[3].motorTorque = 0.0f;
	}
	else
	{				

		wheelsCollider[0].brakeTorque = 0f;	
		wheelsCollider[1].brakeTorque = 0f;	
		wheelsCollider[2].brakeTorque = 0f;	
		wheelsCollider[3].brakeTorque = 0f;	
	}
}
void driveBackwards() {
	
	for (int i = 0; i < 4; i++)
	{
		float accelarateBack  = 1.0f;		
		
		wheelsCollider[i].motorTorque = 0;
		wheelsCollider[i].motorTorque -= accelarateBack * maxTorque;
		
		timer -= Time.deltaTime;
	
		if (timer <= 0)
		{
			timer = 0;		
			isRunning = true;
			timer = resetTimer;
			wheelsCollider[i].motorTorque = 0;
		}
	}	
}*/
}
