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
            SpeedText.text = "Speed: " + Speed().ToString("f0") + " km/h";
    }

    private float Speed()
    {
        //var collider = control.wheelPairs.First(x => x.motor).leftWheelColider;
        //return collider.radius * Mathf.PI * collider.rpm * 60f / 1000f;
        var rb = GetComponent<Rigidbody>();
        return rb.velocity.magnitude * 3.6f;
    }
}