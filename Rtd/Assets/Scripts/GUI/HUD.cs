using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Car;

public class HUD : MonoBehaviour {

    public CarSpirit spirit;
    public CarControl control;
    Image img;
    public Image[] images;

    void FixedUpdate()  {
        //SET hp 
        Text hp = GameObject.Find("Hp").GetComponent<Text>();
        hp.text = string.Format("{0}/{1}", spirit.Hp, spirit.MaxHp);
        //set spEED
        Text speed = GameObject.Find("Speed").GetComponent<Text>();
        speed.text = string.Format("{0} km/h", control.Speed.ToString("0"));
        //SET IMAGE
        //img.enabled = false;
        //img = images[];
    }
}