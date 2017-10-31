using Assets.Scripts.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarInfo
{
    public WheelCollider leftWheelColider;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheelColider;
    public GameObject rightWheelMesh;

    public bool motor;
    public bool steering;
}

public class CarControl : MonoBehaviour
{
    public float maxMotorTorque;
    public float maxSteeringAngle;
    [SerializeField]
    private float motorTorque;
    [SerializeField]
    private float steerAngle;

    [SerializeField]
    private Quaternion rot;
    [SerializeField]
    private Vector3 pos;

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

    public void Update()
    {
        motorTorque = maxMotorTorque * Input.GetAxis(AxisNames.Vertical);
        steerAngle = maxSteeringAngle * Input.GetAxis(AxisNames.Horizontal);
        float brakeTorque = Mathf.Abs(Input.GetAxis(AxisNames.Jump));
        if (brakeTorque > 0.001)
        {
            brakeTorque = maxMotorTorque;
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
                wheelPair.leftWheelColider.steerAngle = wheelPair.rightWheelColider.steerAngle = Math.Sign(motorTorque) * steerAngle;
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
