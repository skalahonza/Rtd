using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.Constants;
using UnityEngine;

public class ControlWheel : MonoBehaviour
{

    public Transform[] Wheels;

    public float MotorPower = 5.0f;
    public float MaxMotorTorque = 400;
    public float MaxSteeringAngle = 10f;
    public float TurningRadius = 3.5f;
    public float maxVelocity = 150f;
    public float turnCoeficient = 3f;

    [SerializeField]
    private float motorTorque = 0.0f;
    private float brake = 0.0f;
    private float wheelTurn = 0.0f;


    private Rigidbody carRigidbody;
    // Use this for initialization
    void Start()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0.3f);
    }

    protected void Move()
    {
        motorTorque = Input.GetAxis(AxisNames.Vertical) * MotorPower * carRigidbody.mass;
        wheelTurn = Input.GetAxis(AxisNames.Horizontal) * turnCoeficient * MaxSteeringAngle * carRigidbody.mass * Math.Sign(motorTorque);
        brake = Input.GetKey(KeyCode.Space) ? carRigidbody.mass * 0.1f : 0.0f;

        //front wheels visual steering
        for (int i = 0; i < 2; i++)
        {
            //turn collider
            GetCollider(i).steerAngle = wheelTurn;
        }

        for (int i = 0; i < Wheels.Length; i++)
        {
            //spin wheels
            Wheels[i].Rotate(0, carRigidbody.velocity.magnitude, 0);

            //breaking
            if (brake > 0.0f)
            {
                GetCollider(i).brakeTorque = brake;
                GetCollider(i).motorTorque = 0.0f;
            }

            //not breaking
            else
            {
                GetCollider(i).brakeTorque = 0.0f;

                //steer if not standing
                carRigidbody.AddRelativeTorque(SteerMechanics.Steer(carRigidbody.velocity, wheelTurn, TurningRadius));

                //accelerate if bellow maxVelocity
                if (carRigidbody.velocity.magnitude < maxVelocity)
                    carRigidbody.AddRelativeForce(0f, 0f, motorTorque);

                GetCollider(i).motorTorque = motorTorque;
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    WheelCollider GetCollider(int n)
    {
        return Wheels[n].gameObject.GetComponent<WheelCollider>();
    }

    WheelCollider GetCollider(Transform wheel)
    {
        return wheel.gameObject.GetComponent<WheelCollider>();
    }
}
