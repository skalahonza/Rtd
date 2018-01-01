using System;
using Assets.Scripts.Car;
using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.States.Camera;
using UnityStandardAssets.Utility;

public class LocalPlayer : Player
{

    private CarSpirit spirit;
    private CarControl control;
    private SmoothFollow smf;
    private CameraState cameraState = new MediumView();

    void Start()
    {
        //attach cam here
        smf = GameObject.FindObjectOfType<SmoothFollow>();
        smf.target = this.transform;
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();
        cameraState.SetUp(smf);
    }

    private void Update()
    {
        if (!startRace)
            return;
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

        control.setUpdate(spirit.MaxMotorTorque * Input.GetAxis(AxisNames.Vertical)
        , spirit.MaxSteeringAngle * Input.GetAxis(AxisNames.Horizontal)
        , Mathf.Abs(Input.GetAxis(AxisNames.Jump)));
    }
}