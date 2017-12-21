using Assets.Scripts.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarInfo
{
    public WheelCollider leftWheelColider;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheelColider;
    public GameObject rightWheelMesh;

    public bool motor;
    public bool steering;
}

[RequireComponent(typeof(CarSpirit))]
public class CarControl : MonoBehaviour
{
    [SerializeField]
    private float motorTorque;
    [SerializeField]
    private float steerAngle;

    float brakeTorque ;
    private Quaternion rot;
    private Vector3 pos;

    private CarSpirit spirit;

    public List<CarInfo> wheelPairs;

    public void VisualizeWheel(CarInfo wheelPair)
    {        
        wheelPair.leftWheelColider.GetWorldPose(out pos, out rot);
        wheelPair.leftWheelMesh.transform.position = pos;
        wheelPair.leftWheelMesh.transform.rotation = rot;

        wheelPair.rightWheelColider.GetWorldPose(out pos, out rot);
        wheelPair.rightWheelMesh.transform.position = pos;
        wheelPair.rightWheelMesh.transform.rotation = rot;
    }

    void Start()
    {
        spirit = GetComponent<CarSpirit>();
        Vector3 vec = GetComponent<Rigidbody>().centerOfMass;
        GetComponent<Rigidbody>().centerOfMass = new Vector3(vec[0], -0.1f,vec[2]);
    }

    public void setUpdate(float motorTorque, float steerAngle, float brakeTorque){
        this.motorTorque = motorTorque;
        this.steerAngle = steerAngle;
        this.brakeTorque = brakeTorque;
    }

    public void Update()
    {
        if (brakeTorque > 0.001)
        {
            brakeTorque = spirit.MaxMotorTorque;
            motorTorque = 0;
        }
        else
        {
            brakeTorque = 0;
        }

        foreach (CarInfo wheelPair in wheelPairs)
        {
            //steering wheels
            if (wheelPair.steering)
            {
                wheelPair.leftWheelColider.steerAngle = wheelPair.rightWheelColider.steerAngle = steerAngle;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                // Check if the car is reversing
                if ((transform.forward.normalized - GetComponent<Rigidbody>().velocity.normalized).magnitude == 0)
                {
                    wheelPair.leftWheelColider.steerAngle *= Math.Sign(motorTorque);
                }
            }

            // motored wheel pair
            if (wheelPair.motor)
            {
                wheelPair.leftWheelColider.motorTorque = motorTorque;
                wheelPair.rightWheelColider.motorTorque = motorTorque;
            }

            wheelPair.leftWheelColider.brakeTorque = brakeTorque;
            wheelPair.rightWheelColider.brakeTorque = brakeTorque;

            VisualizeWheel(wheelPair);
        }
    }
}
