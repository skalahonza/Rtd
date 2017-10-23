using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constants;
using UnityEngine;

public class ControlWheel : MonoBehaviour
{

    public Transform[] Wheels;

    public float MotorPower = 150.0f;
    public float MaxTurn = 25.0f;

    private float instantPower = 0.0f;
    private float brake = 0.0f;
    private float wheelTurn = 0.0f;

    private Rigidbody carRigidbody;
	// Use this for initialization
	void Start ()
	{
	    carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = new Vector3(0,-0.5f,0.3f);

	}

    void FixedUpdate()
    {
        instantPower = Input.GetAxis(AxisNames.Vertical) * MotorPower * Time.deltaTime;
        wheelTurn = Input.GetAxis(AxisNames.Horizontal) * MaxTurn;
        brake = Input.GetKey(KeyCode.Space) ? carRigidbody.mass * 0.1f : 0.0f;

/*        for (int i = 0; i < 2; i++)
        {
            //turn collider
            GetCollider(i).steerAngle = wheelTurn;

            //turn wheel
            Wheels[i].localEulerAngles = new Vector3(Wheels[i].localEulerAngles.x,
                GetCollider(i).steerAngle - Wheels[i].localEulerAngles.z + 90,
                Wheels[i].localEulerAngles.z
                );

            //spin wheels
            Wheels[i].Rotate(0, -GetCollider(i).rpm/60*360*Time.deltaTime,0);

            //breaks
            if (brake > 0.0f)
            {
                GetCollider(i).brakeTorque = brake;
                GetCollider(i).motorTorque = 0.0f;
            }
            else
            {
                GetCollider(i).brakeTorque = 0.0f;
                GetCollider(i).motorTorque = instantPower;
            }
        }*/
    }

    WheelCollider GetCollider(int n)
    {
        return Wheels[n].gameObject.GetComponent<WheelCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
