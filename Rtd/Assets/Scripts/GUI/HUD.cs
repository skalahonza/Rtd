using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Car;

public class HUD : MonoBehaviour {

    public CarSpirit spirit;
    public CarControl control;
    public GameObject menu;
    public delegate void FinishPressed();
    FinishPressed func;
    Image pwup;
    Text speed;
    Text hp;
    Text pos;
    private void Start() {
        hp = GameObject.Find("Hp").GetComponent<Text>();
        speed = GameObject.Find("Speed").GetComponent<Text>();
        pwup = GameObject.Find("Pwup").GetComponent<Image>();
        pos = GameObject.Find("position").GetComponent<Text>();
    }

    public void setDelegate(FinishPressed finish){
        func = finish;
    }

    void FixedUpdate()  {
        //SET hp 
        hp.text = string.Format("{0}/{1}", spirit.Hp, spirit.MaxHp);
        //set spEED
        speed.text = string.Format("{0} km/h", control.Speed.ToString("0"));
        //SET IMAGE
        if(spirit._powerUp != null){
            pwup.sprite = spirit._powerUp.GetPowerupIcon();
            pwup.enabled = true;
        }else{
            pwup.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
             menu.SetActive(true);
        }
    }

    public void FinishGame(){
        func();
    }

    public void CloseApp(){
        Application.Quit();
    }
    
    public void CloseMenu(){
        menu.SetActive(false);
    }

}