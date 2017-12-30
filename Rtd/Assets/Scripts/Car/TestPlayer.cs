using Assets.Scripts.Constants;
using UnityEngine;
using UnityStandardAssets.Utility;

/// <summary>
/// This class is only ment for testing of driving in 
/// </summary>
public class TestPlayer : MonoBehaviour
{
    private CarSpirit spirit;
    private CarControl control;

    void Start()
    {
        //attach cam here
        var smf = GameObject.FindObjectOfType<SmoothFollow>();
        smf.target = this.transform;
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            spirit.UsePowerUp();
        }
        control.setUpdate(spirit.MaxMotorTorque * Input.GetAxis(AxisNames.Vertical)
            , spirit.MaxSteeringAngle * Input.GetAxis(AxisNames.Horizontal)
            , Mathf.Abs(Input.GetAxis(AxisNames.Jump)));
    }
}