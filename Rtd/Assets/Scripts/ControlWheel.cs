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
    private float _break = 0.0f;
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
        _break = Input.GetKey(KeyCode.Space) ? carRigidbody.mass * 0.1f : 0.0f;

        for (int i = 0; i < 2; i++)
        {
            getCollider(i).steerAngle = wheelTurn;

            
        }

        getCollider(0).steerAngle = wheelTurn;
        getCollider(1).steerAngle = wheelTurn;

    }

    WheelCollider getCollider(int n)
    {
        return Wheels[n].gameObject.GetComponent<WheelCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
