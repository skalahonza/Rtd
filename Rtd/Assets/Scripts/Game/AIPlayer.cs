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
        agent.speed = /*spirit.maxSpeed/2*/30;
        agent.angularSpeed = 30;
        agent.height = 5.5f;
        agent.radius = 8.0f;
        //set offset
        agent.baseOffset = 1.2f;
    }

    void FixedUpdate() {
        if(!startRace)
            return;
        if(map.checkpoints.Length != checkpointOffest+1){
            agent.destination = map.checkpoints[checkpointOffest+1].transform.position;
        }
        
	    //GetSteer ();
	    //Move ();
	    //Breaking ();	
        if(!agent.isOnNavMesh){
            Respawn(false);
        }
    }
/*
    void GetSteer () {

	Vector3 steerVector  = transform.InverseTransformPoint(Vector3(path[currentPathObj].position.x,
								transform.position.y,path[currentPathObj].position.z));
	float newSteer  = maxSteer * (steerVector.x / steerVector.magnitude);
	
	wheelsCollider [0].steerAngle = newSteer;
	wheelsCollider [1].steerAngle = newSteer;	
	
	if (steerVector.magnitude <= distFromPath)
	{	
		currentPathObj++;
		
		if (currentPathObj >= path.length)
		{
			currentPathObj = 0;			
			
			var randomValue  = Random.Range(0,2);
		
			if (randomValue == 0)
			{
				GetPath ();
			    //Debug.Log("Im in path 1");
			}		
			
			if (randomValue == 1)
			{			
				GetPath2 ();
				//Debug.Log("Im in path 2");
			}		
		}		
	}	
		
	if (Math.Abs (newSteer) >= 8 && GetComponent<Rigidbody>().velocity.magnitude >= 15)
	{					
		if (distFromPath)
		{
			isBreaking = true;
		}
	}
	else
	{
		isBreaking = false;
	}																																																																							
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
		breakLights.SetActive(true);
		
		wheelsCollider[0].brakeTorque = breakForce;	
		wheelsCollider[1].brakeTorque = breakForce;	
		wheelsCollider[2].brakeTorque = breakForce;	
		wheelsCollider[3].brakeTorque = breakForce;
		
		wheelsCollider[0].motorTorque = 0.0;	
		wheelsCollider[1].motorTorque = 0.0;	
		wheelsCollider[2].motorTorque = 0.0;	
		wheelsCollider[3].motorTorque = 0.0;
	}
	else
	{				
		breakLights.SetActive(false);
		
		wheelsCollider[0].brakeTorque = 0;	
		wheelsCollider[1].brakeTorque = 0;	
		wheelsCollider[2].brakeTorque = 0;	
		wheelsCollider[3].brakeTorque = 0;	
	}
}
*/
}
