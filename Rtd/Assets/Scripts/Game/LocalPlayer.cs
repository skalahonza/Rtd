using System;
using Assets.Scripts.Car;
using UnityEngine;
using Assets.Scripts.Constants;
using UnityStandardAssets.Utility;

public class LocalPlayer : Player
{

    private CarSpirit spirit;
    private CarControl control;
    private SmoothFollow smf;

    void Start()
    {
        //attach cam here
        smf = GameObject.FindObjectOfType<SmoothFollow>();
        smf.target = this.transform;
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();
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
            //TODO change camera view
        }

        control.setUpdate(spirit.MaxMotorTorque * Input.GetAxis(AxisNames.Vertical)
        , spirit.MaxSteeringAngle * Input.GetAxis(AxisNames.Horizontal)
        , Mathf.Abs(Input.GetAxis(AxisNames.Jump)));
    }
}