using UnityEngine;
using Assets.Scripts.Constants;
using UnityStandardAssets.Utility;

public class LocalPlayer : Player {

    private CarSpirit spirit;
    private CarControl control;

    void Start()
    {
        //attach cam here
        SmoothFollow smf = GameObject.FindObjectOfType<SmoothFollow>();
        smf.target = this.transform;
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();
    }

    private void Update() {
        if(!startRace)
            return;
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            spirit.UsePowerUp();
        }    
        control.setUpdate(spirit.MaxMotorTorque * Input.GetAxis(AxisNames.Vertical)
        , spirit.MaxSteeringAngle * Input.GetAxis(AxisNames.Horizontal)
        ,Mathf.Abs(Input.GetAxis(AxisNames.Jump)));
    }
}