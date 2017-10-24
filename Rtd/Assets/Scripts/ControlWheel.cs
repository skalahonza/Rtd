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
    void Start()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0.3f);
    }

    void FixedUpdate()
    {
        instantPower = Input.GetAxis(AxisNames.Vertical) * MotorPower * Time.deltaTime;
        wheelTurn = Input.GetAxis(AxisNames.Horizontal) * MaxTurn;
        brake = Input.GetKey(KeyCode.Space) ? carRigidbody.mass * 0.1f : 0.0f;

        //front wheels
        for (int i = 0; i < 2; i++)
        {
            //turn collider
            GetCollider(i).steerAngle = wheelTurn;

            //turn wheels
            Wheels[i].localEulerAngles = new Vector3(GetCollider(i).steerAngle, 
                Wheels[i].localEulerAngles.y,                
                Wheels[i].localEulerAngles.z
            );
        }

        for (int i = 0; i < Wheels.Length; i++)
        {
            //spin wheels
            Wheels[i].Rotate(0, -GetCollider(i).rpm / 60 * 360 * Time.deltaTime, 0);

            //breaking
            if (brake > 0.0f)
            {
                GetCollider(i).brakeTorque = brake;

                //TODO refector
                GetCollider(2).motorTorque = 0.0f;
                GetCollider(3).motorTorque = 0.0f;
            }

            //not breaking
            else
            {
                GetCollider(i).brakeTorque = 0.0f;
                transform.position += Vector3.forward * Time.deltaTime * instantPower;                


                //TODO refector
                GetCollider(2).motorTorque = instantPower;
                GetCollider(3).motorTorque = instantPower;
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
