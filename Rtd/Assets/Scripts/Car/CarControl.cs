using System;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

namespace Assets.Scripts.Car
{
    [RequireComponent(typeof(CarSpirit))]
    public class CarControl : MonoBehaviour
    {
        [SerializeField]
        private float motorTorque;
        [SerializeField]
        private float steerAngle;    

        float brakeTorque;
        private Quaternion rot;
        private Vector3 pos;

        private CarSpirit spirit;

        public List<CarInfo> wheelPairs;

        private Rigidbody rb;

        public float Speed { get { return rb.velocity.magnitude *3.6f; } }

        public void VisualizeWheel(CarInfo wheelPair)
        {
            var WheelL = wheelPair.leftWheelColider;
            var WheelR = wheelPair.rightWheelColider;
            float AntiRoll = 5000.0f;
            var rb = GetComponent<Rigidbody>();

            WheelHit hit;
            var travelL = 1.0;
            var travelR = 1.0;

            var groundedL = WheelL.GetGroundHit(out hit);
            if (groundedL)
                travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

            var groundedR = WheelR.GetGroundHit(out hit);
            if (groundedR)
                travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

            float antiRollForce = (float)((travelL - travelR) * AntiRoll);

            if (groundedL)
                rb.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                    WheelL.transform.position);
            if (groundedR)
                rb.AddForceAtPosition(WheelR.transform.up * antiRollForce,
                    WheelR.transform.position);

            WheelL.GetWorldPose(out pos, out rot);
            wheelPair.leftWheelMesh.transform.position = pos;
            wheelPair.leftWheelMesh.transform.rotation = rot;

            WheelR.GetWorldPose(out pos, out rot);
            wheelPair.rightWheelMesh.transform.position = pos;
            wheelPair.rightWheelMesh.transform.rotation = rot;
        }

        void Start()
        {
            spirit = GetComponent<CarSpirit>();
            rb = GetComponent<Rigidbody>();
            rb.ResetCenterOfMass();
            rb.centerOfMass += new Vector3(0, -0.5f, 0);
        }

        public void setUpdate(float motorTorque, float steerAngle, float brakeTorque)
        {
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

            foreach (var wheelPair in wheelPairs)
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
                    float scaledTorque = motorTorque;
                    if (wheelPair.leftWheelColider.rpm < 0)//reversing
                    {
                        if (Speed < spirit.maxReverseSpeed)
                            scaledTorque = Mathf.Lerp(scaledTorque, 0, wheelPair.leftWheelColider.rpm / spirit.maxRPM);
                        else scaledTorque = 0;

                        //TODO ENGINE SOUND
                        var audio = GetComponent<AudioSource>();                        
                        audio.pitch = 1 + Speed / spirit.maxReverseSpeed;
                    }
                    else// going forward
                    {
                        if (Speed < spirit.maxSpeed)
                            scaledTorque = Mathf.Lerp(scaledTorque, 0, wheelPair.leftWheelColider.rpm / spirit.maxRPM);
                        else scaledTorque = 0;

                        //TODO ENGINE SOUND
                        var audio = GetComponent<AudioSource>();
                        audio.pitch = 1 + Speed / spirit.maxSpeed;
                    }

                    wheelPair.leftWheelColider.motorTorque = scaledTorque;
                    wheelPair.rightWheelColider.motorTorque = scaledTorque;                    
                }

                // apply breaking
                wheelPair.leftWheelColider.brakeTorque = brakeTorque;
                wheelPair.rightWheelColider.brakeTorque = brakeTorque;

                VisualizeWheel(wheelPair);
            }
        }
    }
}