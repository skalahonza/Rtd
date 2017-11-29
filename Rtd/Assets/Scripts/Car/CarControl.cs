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

    [SerializeField]
    private Quaternion rot;
    [SerializeField]
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
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            spirit.UsePowerUp();
        }

        var maxMotorTorque = spirit.MaxMotorTorque;
        var maxSteeringAngle = spirit.MaxSteeringAngle;

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
