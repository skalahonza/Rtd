using System;
using UnityEngine;

namespace Assets.Scripts.Car
{
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
}