using System;
using Assets.Mechanics;
using Assets.Scripts.Car;
using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Powerups;
using Assets.Scripts.States.Camera;
using UnityStandardAssets.Utility;
using UnityEngine.SceneManagement;

public class LocalPlayer : Player
{

    private CarSpirit spirit;
    private CarControl control;
    private SmoothFollow smf;
    private CameraState cameraState = new MediumView();
    private IPowerup previousPowerup;

    void Start()
    {
        //attach cam here
        smf = GameObject.FindObjectOfType<SmoothFollow>();
        smf.target = this.transform;
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();
        cameraState.SetUp(smf);
        HUD hud = GameObject.FindObjectOfType<HUD>();
        hud.spirit = spirit;
        hud.control = control;
    }

    private void Update()
    {
        if (!startRace)
            return;

        var currentPowerup = spirit._powerUp;
        if (currentPowerup != previousPowerup && currentPowerup != null)
        {
            SoundMechanics.SpawnSound("powerup_spawn");
        }
        previousPowerup = currentPowerup;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            spirit.UsePowerUp();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn(false);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            //change camera view
            cameraState = cameraState.NextState();
            cameraState.SetUp(smf);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //drop unwanted powerup
            spirit._powerUp = null;
        }

        control.setUpdate(spirit.MaxMotorTorque * Input.GetAxis(AxisNames.Vertical)
        , spirit.MaxSteeringAngle * Input.GetAxis(AxisNames.Horizontal)
        , Mathf.Abs(Input.GetAxis(AxisNames.Jump)));

        //car control mechanique
        if (control.brakeTorque > 0.001)
        {
            control.brakeTorque = spirit.MaxMotorTorque;
            control.motorTorque = 0;
        }
        else
        {
            control.brakeTorque = 0;
        }

        foreach (var wheelPair in control.wheelPairs)
        {
            //steering wheels
            if (wheelPair.steering)
            {
                wheelPair.leftWheelColider.steerAngle = wheelPair.rightWheelColider.steerAngle = control.steerAngle;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                // Check if the car is reversing
                if ((transform.forward.normalized - GetComponent<Rigidbody>().velocity.normalized).magnitude == 0)
                {
                    wheelPair.leftWheelColider.steerAngle *= Math.Sign(control.motorTorque);
                }
            }

            // motored wheel pair
            if (wheelPair.motor)
            {
                float scaledTorque = control.motorTorque;
                if (wheelPair.leftWheelColider.rpm < 0)//reversing
                {
                    if (control.Speed < spirit.maxReverseSpeed)
                        scaledTorque = Mathf.Lerp(scaledTorque, 0, wheelPair.leftWheelColider.rpm / spirit.maxRPM);
                    else scaledTorque = 0;

                    //ENGINE SOUND
                    var audio = GetComponent<AudioSource>();
                    audio.pitch = 1 + control.Speed / spirit.maxReverseSpeed;
                }
                else// going forward
                {
                    if (control.Speed < spirit.maxSpeed)
                        scaledTorque = Mathf.Lerp(scaledTorque, 0, wheelPair.leftWheelColider.rpm / spirit.maxRPM);
                    else scaledTorque = 0;

                    //ENGINE SOUND
                    var audio = GetComponent<AudioSource>();
                    audio.pitch = 1 + control.Speed / spirit.maxSpeed;
                }

                wheelPair.leftWheelColider.motorTorque = scaledTorque;
                wheelPair.rightWheelColider.motorTorque = scaledTorque;
            }

            // apply breaking
            wheelPair.leftWheelColider.brakeTorque = control.brakeTorque;
            wheelPair.rightWheelColider.brakeTorque = control.brakeTorque;

            VisualizeWheel(wheelPair);
        }
    }

    private void VisualizeWheel(CarInfo wheelPair)
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

        Vector3 pos;
        Quaternion rot;
        WheelL.GetWorldPose(out pos, out rot);
        wheelPair.leftWheelMesh.transform.position = pos;
        wheelPair.leftWheelMesh.transform.rotation = rot;

        WheelR.GetWorldPose(out pos, out rot);
        wheelPair.rightWheelMesh.transform.position = pos;
        wheelPair.rightWheelMesh.transform.rotation = rot;
    }
}