using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Car
{
    /// <inheritdoc />
    /// <summary>
    /// Component is used for applying car physics
    /// </summary>
    [RequireComponent(typeof(CarSpirit))]
    public class CarControl : MonoBehaviour
    {
        [SerializeField]
        public float motorTorque;
        [SerializeField]
        public float steerAngle;

        public float brakeTorque;
        private Quaternion rot;
        private Vector3 pos;

        public List<CarInfo> wheelPairs;

        private Rigidbody rb;

        public float Speed { get { return rb.velocity.magnitude *3.6f; } }

        /// <summary>
        /// Move wheel meshes onto colliders
        /// </summary>
        /// <param name="wheelPair">Wheel pair to visualize</param>
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
            rb = GetComponent<Rigidbody>();
            rb.ResetCenterOfMass();
            rb.centerOfMass += new Vector3(0, -0.5f, 0);
        }

        /// <summary>
        /// Used for driving by human player
        /// </summary>
        /// <param name="motorTorque">Vertical axis * MaxMotorTorque</param>
        /// <param name="steerAngle"></param>
        /// <param name="brakeTorque"></param>
        public void setUpdate(float motorTorque, float steerAngle, float brakeTorque)
        {
            this.motorTorque = motorTorque;
            this.steerAngle = steerAngle;
            this.brakeTorque = brakeTorque;
        }

        /// <summary>
        /// Used for AI driving
        /// </summary>
        /// <param name="steerAngle">Angle the car rotated</param>
        /// <param name="velocity">Velocity calculated by agent</param>
        public void setUpdate(float steerAngle, Vector3 velocity)
        {
            rb.velocity = velocity;
            this.steerAngle = steerAngle;
        }
    }
}