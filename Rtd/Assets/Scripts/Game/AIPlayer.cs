using System;
using System.Collections.Generic;
using Assets.Scripts.Car;
using UnityEngine;

class AIPlayer : Player {
	CarControl control;
	public List<Vector3> path = new List<Vector3> ();
	private int pathIndex;
	float distFromPath = 50.0f;
	bool reversing = false;
	public bool inSector;
	float sensorLength = 50.0f;
	float frontSensorStartPoint = 4.0f;
	float frontSensorSideDist = 2.0f;
	float frontSensorsAngle = 30.0f;
	float sidewaySensorLength = 35.0f;
	private static System.Random rand = new System.Random ();
	private int flag = 0;
	float reverCounter = 0.0f;
	float currentSpeed;
	float waitToReverse = 2.0f;
	float reverFor = 2.5f;
	CarInfo frontWheelpair;
	CarInfo backWheelPair;
	CarSpirit spirit;

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 0, 0, 0.75F);
		Vector3 prev = new Vector3();
		int i =0;
		foreach(var pt in path){
				Gizmos.DrawLine(prev, pt);
			if(pathIndex == i)
				 Gizmos.color = new Color(1, 1, 0, 0.80F);
        	Gizmos.DrawSphere(pt, 3.0f);
			Gizmos.color = new Color(1, 0, 0, 0.75F);
			prev= pt;
			i++;
		}
    }

	void Start () {
        base.Start();
		spirit = GetComponent<CarSpirit> ();
		control = GetComponent<CarControl> ();
		agent.autoBraking = false;
		agent.radius = 8.0f;
		agent.speed = 20.0f;
		agent.acceleration = 0.5f;
		if (control.wheelPairs[1].steering) {
			frontWheelpair = control.wheelPairs[1];
			backWheelPair = control.wheelPairs[0];
		} else {
			frontWheelpair = control.wheelPairs[0];
			backWheelPair = control.wheelPairs[1];
		}
		spirit.MaxSteeringAngle = 15.0f;
	}

	void Sensors () {
		flag = 0;
		float avoidSenstivity = 0.0f;
		Vector3 pos;
		RaycastHit hit;
		var rightAngle = Quaternion.AngleAxis (frontSensorsAngle, transform.up) * transform.forward;
		var leftAngle = Quaternion.AngleAxis (-frontSensorsAngle, transform.up) * transform.forward;

		pos = transform.position;
		pos.y += UnityEngine.Random.Range(0.2f, 1.2f);
		pos += transform.forward * frontSensorStartPoint;

		//BRAKING SENSOR

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
				flag++;
				backWheelPair.leftWheelColider.brakeTorque = spirit.MaxMotorTorque * 0.05f;
				backWheelPair.rightWheelColider.brakeTorque = spirit.MaxMotorTorque * 0.05f;
				Debug.DrawLine (pos, hit.point, Color.red);
			}
		} else {
			backWheelPair.leftWheelColider.brakeTorque = 0.0f;
			backWheelPair.rightWheelColider.brakeTorque = 0.0f;
		}

		//Front Straight Right Sensor
		pos += transform.right * frontSensorSideDist;

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
				flag++;
				avoidSenstivity -= Mathf.Lerp(0.0f, 1.0f, hit.distance/sensorLength);
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		} else if (Physics.Raycast (pos, rightAngle, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
				avoidSenstivity -= Mathf.Lerp(0.0f, 0.5f, hit.distance/sensorLength);
				flag++;
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		}

		//Front Straight left Sensor
		pos = transform.position;
		pos += transform.forward * frontSensorStartPoint;
		pos -= transform.right * frontSensorSideDist;

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
				flag++;
				avoidSenstivity += Mathf.Lerp(0.0f, 1.0f, hit.distance/sensorLength);
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		} else if (Physics.Raycast (pos, leftAngle, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
				flag++;
				avoidSenstivity += Mathf.Lerp(0.0f, 0.5f, hit.distance/sensorLength);
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		}

		//Right SideWay Sensor
		if (Physics.Raycast (transform.position, transform.right, out hit, sidewaySensorLength)) {
			if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
				flag++;
				avoidSenstivity -= Mathf.Lerp(0.0f, 0.5f, hit.distance/sensorLength);
				Debug.DrawLine (transform.position, hit.point, Color.white);
			}
		}

		//Left SideWay Sensor
		if (Physics.Raycast (transform.position, -transform.right, out hit, sidewaySensorLength)) {
			if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
				flag++;
				avoidSenstivity += Mathf.Lerp(0.0f, 0.5f, hit.distance/sensorLength);
				Debug.DrawLine (transform.position, hit.point, Color.white);
			}
		}

		pos = transform.position;
		pos += transform.forward * frontSensorStartPoint;
		//Front Mid Sensor
		if (avoidSenstivity == 0.0f) {

			if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
				if (hit.transform.tag != "Terrain" && hit.transform.tag != "notAvoid") {
					if (hit.normal.x < 0)
						avoidSenstivity = -Mathf.Lerp(0.0f, 1.0f, hit.distance/sensorLength);
					else
						avoidSenstivity = Mathf.Lerp(0.0f, 1.0f, hit.distance/sensorLength);
					Debug.DrawLine (pos, hit.point, Color.white);
				}
			}
		}

		if (GetComponent<Rigidbody> ().velocity.magnitude < 2.0f && !reversing) {
			reverCounter += Time.deltaTime;
			if (reverCounter >= waitToReverse) {
				reverCounter = 0.0f;
				reversing = true;
			}
		} else if (!reversing) {
			reverCounter = 0.0f;
		}

		if (reversing) {
			avoidSenstivity *= -1;
			reverCounter += Time.deltaTime;
			if (reverCounter >= reverFor) {
				reverCounter = 0.0f;
				reversing = false;
			}
		}

		if (flag != 0)
			AvoidSteer (avoidSenstivity);
	}

	void FixedUpdate () {
		if (!startRace || finished)
			return;
		GetSteer ();
		Move ();
		Sensors ();
		agent.velocity = GetComponent<Rigidbody> ().velocity;
		control.VisualizeWheel (control.wheelPairs[1]);
		control.VisualizeWheel (control.wheelPairs[0]);
		if (spirit._powerUp != null)
			if (rand.Next () % 5 == 0)
				spirit.UsePowerUp ();
		if (!agent.isOnNavMesh) {
			Respawn (false);
		}
	}

	void GetPath () {
		if (!agent.pathPending) {
			foreach (var point in agent.path.corners) {
				path.Add (point);
			}
		}
	}

	void GetSteer () {
		if (pathIndex + 1 >= path.Count) {
			GetPath ();
			pathIndex = 0;
		}

		if (pathIndex + 1 >= path.Count)
			return;
		Vector3 steerVector = transform.InverseTransformPoint (new Vector3 (
			path[pathIndex].x,
			transform.position.y,
			path[pathIndex].z
		));
		float newSteer =  (steerVector.x / steerVector.magnitude);
		newSteer = Mathf.Lerp(0.0f, spirit.MaxSteeringAngle * newSteer,  currentSpeed/spirit.maxSpeed);
		frontWheelpair.leftWheelColider.steerAngle = newSteer;
		frontWheelpair.rightWheelColider.steerAngle = newSteer;
		if (Vector3.Distance (transform.position, path[pathIndex]) <= distFromPath) {
			pathIndex++;
		}
	}

	void Move () {
		currentSpeed = control.Speed;
		currentSpeed = Mathf.Round (currentSpeed);
		if (currentSpeed <= spirit.maxSpeed && !inSector) {
			if (!reversing) {
				float mt = Mathf.Lerp (spirit.MaxMotorTorque, 0, backWheelPair.leftWheelColider.rpm / spirit.maxRPM);
				backWheelPair.leftWheelColider.motorTorque = mt;
				backWheelPair.rightWheelColider.motorTorque = mt;
			} else {
				float mt = Mathf.Lerp (spirit.MaxMotorTorque, 0, backWheelPair.leftWheelColider.rpm / spirit.maxRPM);
				backWheelPair.leftWheelColider.motorTorque = -mt;
				backWheelPair.rightWheelColider.motorTorque = -mt;
			}
			backWheelPair.leftWheelColider.brakeTorque = 0;
			backWheelPair.rightWheelColider.brakeTorque = 0;
		} else if (!inSector) {
			backWheelPair.leftWheelColider.motorTorque = 0;
			backWheelPair.rightWheelColider.motorTorque = 0;
			backWheelPair.leftWheelColider.brakeTorque = spirit.MaxMotorTorque * 5;
			backWheelPair.rightWheelColider.brakeTorque = spirit.MaxMotorTorque * 5;
		}
	}

	void AvoidSteer (float senstivity) {
		float newSteer = Mathf.Lerp(0.0f, spirit.MaxSteeringAngle * senstivity, currentSpeed/spirit.maxSpeed);

		if (control.wheelPairs[0].steering) {
			control.wheelPairs[0].rightWheelColider.steerAngle = control.wheelPairs[0].leftWheelColider.steerAngle = newSteer;
		} else {
			control.wheelPairs[1].rightWheelColider.steerAngle = control.wheelPairs[1].leftWheelColider.steerAngle = newSteer;
		}
	}

}