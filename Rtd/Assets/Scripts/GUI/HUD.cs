using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Car;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

    public CarSpirit spirit;
    public CarControl control;
    public GameObject menu;
    public delegate void FinishPressed();
    FinishPressed func;
    Game go;
    Image pwup;
    Text speed;
    Text hp;
    public GameObject leaderb;
    bool mact = false;
    Text pos;
    int position = 1;
    public Player player;
    bool isInTransition =false;

    private void Start() {
        go = GameObject.Find("GameObject").GetComponent<Game>();
        hp = GameObject.Find("Hp").GetComponent<Text>();
        speed = GameObject.Find("Speed").GetComponent<Text>();
        pwup = GameObject.Find("Pwup").GetComponent<Image>();
        pos = GameObject.Find("position").GetComponent<Text>();
    }

    public void setDelegate(FinishPressed finish){
        func = finish;
    }

    void FixedUpdate()  {
        if(player.finished){
            RenderLeaderboards();
            return;
        }
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
        if (Input.GetKeyDown(KeyCode.Escape) && !isInTransition) {
            isInTransition = true;
            if(mact){
                gameObject.SetActive(false);
                menu.SetActive(false);
                mact = false;
                gameObject.SetActive(true);
            }else{
                menu.SetActive(true);
                mact = true;
            }
            StartCoroutine(WaitDisableTransition(0.2f));
        }
        UpdatePosition();
        switch (position)
        {
            case 1:
            pos.text = "1st";
            break;
            case 2:
            pos.text = "2nd";
            break;
            case 3:
            pos.text = "3rd";
            break;
            case 4:
            pos.text = "4th";
            break;
            case 5:
            pos.text = "5th";
            break;
            default:
            pos.text = "WTF";
            break;
        }
    }

    public void UpdatePosition(){
        float d = player.GetPathLength();
        int pos = 1;
        if(d == 0.0f){
            return;
        }
        foreach(var pld in go.cars){
            Player pl = pld.GetComponent<Player>();
            if(pl == player)
                continue;
            float semi = pl.GetPathLength();
            if(semi == 0.0f)
                return;
            if((semi < d && pl.checkpointOffest == player.checkpointOffest) || pl.checkpointOffest > player.checkpointOffest )
                pos ++;
        }
        position = pos;
    }

    public void FinishGame(){
        func();
    }

    public void CloseApp(){
        Application.Quit();
    }
    
    public void CloseMenu(){
        gameObject.SetActive(false);
        menu.SetActive(false);
        gameObject.SetActive(true);
    }

    public void RenderLeaderboards(){
        gameObject.SetActive(false); //disable all children instead and activate return button
        leaderb.transform.GetChild(0).gameObject.SetActive(true);

        if(Assets.Mechanics.MultiplayerHelper.IsMultiplayer()){
            leaderb.transform.GetChild(1).gameObject.SetActive(true);
        }else{
            leaderb.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void ReturnBtt(){
        if(Assets.Mechanics.MultiplayerHelper.IsMultiplayer()){
            GameObject.FindObjectOfType<Lobby>().SendReturnToLobby();
        }else{
            SceneManager.LoadScene(4);
        }
    }

    IEnumerator WaitDisableTransition(float waitTime)
    {
     yield return new WaitForSeconds(waitTime);
     DisableTransition();
    }
 public void DisableTransition()
 {
     isInTransition = false;
 }

}