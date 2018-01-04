using System.Linq;
using Assets.Scripts.Car;
using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

/// <summary>
/// This class is only ment for testing of driving in 
/// </summary>
public class TestPlayer : MonoBehaviour
{
    private CarSpirit spirit;
    private CarControl control;

    public Text SpeedText;

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

        var speed = "Speed: " + Speed().ToString("f0") + " km/h";
        Debug.Log(speed);
        if (SpeedText != null)
            SpeedText.text = "Speed: " + Speed().ToString("f0") + " km/h" + " RPM: " + Rpm();
    }

    private float Speed()
    {
        var rb = GetComponent<Rigidbody>();
        return rb.velocity.magnitude * 3.6f;
    }

    private float Rpm()
    {
        var torque = control.wheelPairs.First(x => x.motor).rightWheelColider.motorTorque;
        var p = 400;
        return (30 * p) / (Mathf.PI * torque);
    }
}