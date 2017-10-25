using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constants;
using UnityEngine;

public class ControlWheel : MonoBehaviour
{

    public Transform[] Wheels;

    public float MotorPower = 35.0f;
    public float MaxTurn = 25.0f;
    public float maxVelocity = 50f;
    private float instantPower = 0.0f;
    private float brake = 0.0f;
    private float wheelTurn = 0.0f;


    private Rigidbody carRigidbody;
    // Use this for initialization
    void Start()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0.3f);
    }

    void FixedUpdate()
    {
        instantPower = Input.GetAxis(AxisNames.Vertical) * MotorPower * carRigidbody.mass;
        wheelTurn = Input.GetAxis(AxisNames.Horizontal) * MaxTurn * carRigidbody.mass;
        brake = Input.GetKey(KeyCode.Space) ? carRigidbody.mass * 0.1f : 0.0f;

        //front wheels visual steering
        for (int i = 0; i < 2; i++)
        {
            //turn collider
            GetCollider(i).steerAngle = wheelTurn;

            //turn wheels
            Wheels[i].localEulerAngles = new Vector3(GetCollider(i).steerAngle - Wheels[i].localEulerAngles.z,
                Wheels[i].rotation.y,
                Wheels[i].rotation.z
            );
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
                if (carRigidbody.velocity.magnitude > 10)
                    carRigidbody.AddRelativeTorque(0f, wheelTurn * 2, 0f);


                //accelerate if bellow maxVelocity
                if (carRigidbody.velocity.magnitude < maxVelocity)
                    carRigidbody.AddRelativeForce(0f, 0f, instantPower);

                GetCollider(i).motorTorque = instantPower;
            }
        }
    }

    WheelCollider GetCollider(int n)
    {
        return Wheels[n].gameObject.GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
