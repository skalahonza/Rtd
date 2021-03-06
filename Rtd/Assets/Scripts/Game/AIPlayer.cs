using Assets.Scripts.Car;
using UnityEngine;

/// <summary>
/// Class reprezenting AI player - driving
/// </summary>
class AIPlayer : Player {
	CarControl control;
	CarInfo frontWheelpair;
	CarInfo backWheelPair;
	CarSpirit spirit;
	private static readonly System.Random rand = new System.Random ();

    readonly float distFromPath = 60.0f;
	float decellarationSpeed = 50.0f;
	bool isBreaking;
    readonly float sensorLength = 50.0f;
    readonly float frontSensorStartPoint = 0.8f;
    readonly float frontSensorSideDist = 0.9f;
    readonly float frontSensorsAngle = 30.0f;
    readonly float sidewaySensorLength = 30.0f;
    readonly float avoidSpeed = 5.0f;
	private int flag;

	/// <summary>
	/// called when game start from Payler.Start
	/// initialization
	/// </summary>
	public override void GameStart () {
		spirit = GetComponent<CarSpirit> ();
		control = GetComponent<CarControl> ();
		if (control.wheelPairs[1].steering) {
			frontWheelpair = control.wheelPairs[1];
			backWheelPair = control.wheelPairs[0];
		} else {
			frontWheelpair = control.wheelPairs[0];
			backWheelPair = control.wheelPairs[1];
		}
		decellarationSpeed = spirit.MaxMotorTorque;
	}
	/// <summary>
	/// called when race start - after countdown
	/// </summary>
	override protected void OnRaceStart () {

	}

	void FixedUpdate () {
		if (!startRace || finished)
			return;
		if (flag == 0)
			GetSteer ();
		Move ();
		Sensors ();
		control.VisualizeWheel (control.wheelPairs[1]);
		control.VisualizeWheel (control.wheelPairs[0]);
		if (spirit._powerUp != null)
			if (rand.Next () % 5 == 0)
				spirit.UsePowerUp ();
		if (!agent.isOnNavMesh) {
			Respawn (false);
		}
	}

	/// <summary>
	/// check for steering by path
	/// </summary>
	void GetSteer () {
		if (path == null || path.corners.Length == 0)
			return;
		Vector3 steerVector = transform.InverseTransformPoint (
			new Vector3 (path.corners[pathIndex].x,
				transform.position.y,
				path.corners[pathIndex].z)
		);
		float newSteer = spirit.MaxSteeringAngle * (steerVector.x / steerVector.magnitude);
		frontWheelpair.rightWheelColider.steerAngle = newSteer;
		frontWheelpair.leftWheelColider.steerAngle = newSteer;

		if (steerVector.magnitude <= distFromPath) {
			pathIndex++;
			if (pathIndex >= path.corners.Length)
				pathIndex = 0;
		}

	}

	/// <summary>
	/// move forward or backwards
	/// </summary>
	void Move () {
		if (control.Speed <= spirit.maxSpeed) {
			backWheelPair.leftWheelColider.motorTorque = spirit.MaxMotorTorque;
			backWheelPair.rightWheelColider.motorTorque = spirit.MaxMotorTorque;
			frontWheelpair.leftWheelColider.motorTorque = spirit.MaxMotorTorque;
			frontWheelpair.rightWheelColider.motorTorque = spirit.MaxMotorTorque;
			backWheelPair.leftWheelColider.brakeTorque = 0;
			backWheelPair.rightWheelColider.brakeTorque = 0;
			frontWheelpair.leftWheelColider.brakeTorque = 0;
			frontWheelpair.rightWheelColider.brakeTorque = 0;
		} else {
			backWheelPair.leftWheelColider.motorTorque = 0;
			backWheelPair.rightWheelColider.motorTorque = 0;
			frontWheelpair.leftWheelColider.motorTorque = 0;
			frontWheelpair.rightWheelColider.motorTorque = 0;
			backWheelPair.leftWheelColider.brakeTorque = decellarationSpeed;
			backWheelPair.rightWheelColider.brakeTorque = decellarationSpeed;
			frontWheelpair.leftWheelColider.brakeTorque = decellarationSpeed;
			frontWheelpair.rightWheelColider.brakeTorque = decellarationSpeed;
		}
	}
	/// <summary>
	/// check for obstacle avoidance
	/// </summary>
	void Sensors () {
		flag = 0;
		float avoidSenstivity = 0;
		Vector3 pos;
		RaycastHit hit;
		var rightAngle = Quaternion.AngleAxis (frontSensorsAngle, transform.up) * transform.forward;
		var leftAngle = Quaternion.AngleAxis (-frontSensorsAngle, transform.up) * transform.forward;

		pos = transform.position;
		pos.y += 0.8f;
		pos += transform.forward * frontSensorStartPoint;

		//BRAKING SENSOR

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {
				flag++;
				backWheelPair.leftWheelColider.brakeTorque = decellarationSpeed;
				backWheelPair.rightWheelColider.brakeTorque = decellarationSpeed;
				frontWheelpair.leftWheelColider.brakeTorque = decellarationSpeed;
				frontWheelpair.rightWheelColider.brakeTorque = decellarationSpeed;
				Debug.DrawLine (pos, hit.point, Color.red);
			}
		} else {
			backWheelPair.leftWheelColider.brakeTorque = 0;
			backWheelPair.rightWheelColider.brakeTorque = 0;
			frontWheelpair.leftWheelColider.brakeTorque = 0;
			frontWheelpair.rightWheelColider.brakeTorque = 0;
		}

		//Front Straight Right Sensor
		pos += transform.right * frontSensorSideDist;

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {
				flag++;
				avoidSenstivity -= 1;
				Debug.Log ("Avoiding");
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		} else if (Physics.Raycast (pos, rightAngle, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {
				avoidSenstivity -= 0.5f;
				flag++;
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		}

		//Front Straight left Sensor
		pos = transform.position;
		pos += transform.forward * frontSensorStartPoint;
		pos -= transform.right * frontSensorSideDist;

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {
				flag++;
				avoidSenstivity += 1;
				Debug.Log ("Avoiding");
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		} else if (Physics.Raycast (pos, leftAngle, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {
				flag++;
				avoidSenstivity += 0.5f;
				Debug.DrawLine (pos, hit.point, Color.white);
			}
		}

		//Right SideWay Sensor
		if (Physics.Raycast (transform.position, transform.right, out hit, sidewaySensorLength)) {
			if (hit.transform.tag != "Terrain") {
				flag++;
				avoidSenstivity -= 0.5f;
				Debug.DrawLine (transform.position, hit.point, Color.white);
			}
		}

		//Left SideWay Sensor
		if (Physics.Raycast (transform.position, -transform.right, out hit, sidewaySensorLength)) {
			if (hit.transform.tag != "Terrain") {
				flag++;
				avoidSenstivity += 0.5f;
				Debug.DrawLine (transform.position, hit.point, Color.white);
			}
		}

		//Front Mid Sensor
		if (avoidSenstivity == 0) {

			if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
				if (hit.transform.tag != "Terrain") {
					if (hit.normal.x < 0)
						avoidSenstivity = 1;
					else
						avoidSenstivity = -1;
					Debug.DrawLine (pos, hit.point, Color.white);
				}
			}
		}
		if (flag != 0)
			AvoidSteer (avoidSenstivity);

	}

	void AvoidSteer (float senstivity) {
		frontWheelpair.rightWheelColider.steerAngle = avoidSpeed * senstivity;
		frontWheelpair.leftWheelColider.steerAngle = avoidSpeed * senstivity;
		if (control.Speed > spirit.maxSpeed / 0.5f) {
			backWheelPair.leftWheelColider.brakeTorque += decellarationSpeed / 10.0f;
			backWheelPair.rightWheelColider.brakeTorque += decellarationSpeed / 10.0f;
			frontWheelpair.leftWheelColider.brakeTorque += decellarationSpeed / 10.0f;
			frontWheelpair.rightWheelColider.brakeTorque += decellarationSpeed / 10.0f;
		}
	}
}