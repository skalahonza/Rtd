using System.Collections;
using Assets.Scripts.Car;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// class handling HUD events 
/// </summary>
public class HUD : MonoBehaviour {

    public CarSpirit spirit;
    public CarControl control;
    public GameObject menu;
    public delegate void FinishPressed ();
    FinishPressed func;
    Game go;
    Image pwup;
    Text speed;
    Text hp;
    public GameObject lbdesk;
    public GameObject retbt;
    public GameObject wtxt;
    public GameObject leaderb;
    bool mact;
    Text pos;
    int position = 1;
    public Player player;
    bool isInTransition;
    bool ismp;
    NetworkPlayer[] nps;
    public NetworkPlayer mynp;

    /// <summary>
    /// find HUD objects in HUD scene
    /// </summary>
    private void Start () {
        go = FindObjectOfType<Game> ();
        hp = GameObject.Find ("Hp").GetComponent<Text> ();
        speed = GameObject.Find ("Speed").GetComponent<Text> ();
        pwup = GameObject.Find ("Pwup").GetComponent<Image> ();
        pos = GameObject.Find ("position").GetComponent<Text> ();
        ismp = Assets.Mechanics.MultiplayerHelper.IsMultiplayer ();
        if (ismp) {
            nps = FindObjectsOfType<NetworkPlayer> ();
        }
    }

    /// <summary>
    /// set  game cleaning function
    /// </summary>
    /// <param name="finish">
    /// Mainmenu handling - due to multiplayer
    /// </param>
    public void setDelegate (FinishPressed finish) {
        func = finish;
    }

    /// <summary>
    /// update HUD screen
    /// </summary>
    void FixedUpdate () {
        ismp = Assets.Mechanics.MultiplayerHelper.IsMultiplayer ();
        if (player.finished) {
            RenderLeaderboards ();
            return;
        }
        //SET hp 
        hp.text = string.Format ("{0}/{1}", spirit.Hp, spirit.MaxHp);
        //set spEED
        speed.text = string.Format ("{0} km/h", control.Speed.ToString ("0"));
        //SET IMAGE
        if (spirit._powerUp != null) {
            pwup.sprite = spirit._powerUp.GetPowerupIcon ();
            pwup.enabled = true;
        } else {
            pwup.enabled = false;
        }
        if (Input.GetKeyDown (KeyCode.Escape) && !isInTransition) {
            isInTransition = true;
            if (mact) {
                gameObject.SetActive (false);
                menu.SetActive (false);
                mact = false;
                gameObject.SetActive (true);
            } else {
                menu.SetActive (true);
                mact = true;
            }
            StartCoroutine (WaitDisableTransition (0.2f));
        }
        if (ismp) {
            UpdatePositionMP ();
        } else {
            UpdatePosition ();
        }
        switch (position) {
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

    /// <summary>
    /// check for player position Singleplayer
    /// </summary>
    public void UpdatePosition () {
        float d = player.GetPathLength ();
        int pos = 1;
        if (d == 0.0f) {
            return;
        }
        foreach (var pld in go.cars) {
            Player pl = pld.GetComponent<Player> ();
            if (pl == player)
                continue;
            float semi = pl.GetPathLength ();
            if (semi == 0.0f) {
                return;
            }
            if ((semi > d && pl.checkpointOffest == player.checkpointOffest) || pl.checkpointOffest > player.checkpointOffest)
                pos++;
        }
        position = pos;
    }

    /// <summary>
    /// check for player position multiplayer
    /// </summary>
    public void UpdatePositionMP () {
        float d = player.GetPathLength ();
        int pos = 1;
        if (d == 0.0f) {
            Debug.Log ("M zero");
            return;
        }
        foreach (var pl in nps) {
            if (pl == mynp)
                continue;
            float semi = pl.GetPathLength ();
            if (semi == 0.0f) {
                Debug.Log ("O zero");
                return;
            }
            Debug.Log (string.Format ("mine / other {0} / {1}", d, semi));
            if ((semi > d && pl.checkpointOffest == player.checkpointOffest) || pl.checkpointOffest > player.checkpointOffest)
                pos++;
        }
        position = pos;
    }

    /// <summary>
    /// handle menu button - main menu
    /// </summary>
    public void FinishGame () {
        func ();
    }

    /// <summary>
    /// handle menu button - quit
    /// </summary>
    public void CloseApp () {
        Application.Quit ();
    }

    /// <summary>
    /// handle menu button close
    /// </summary>
    public void CloseMenu () {
        gameObject.SetActive (false);
        menu.SetActive (false);
        gameObject.SetActive (true);
    }

    /// <summary>
    /// render leaderboards on finish
    /// </summary>
    public void RenderLeaderboards () {
        gameObject.SetActive (false); //disable all children instead and activate return button
        lbdesk.SetActive (true);
        if (ismp) {
            wtxt.SetActive (true);
        } else {
            retbt.SetActive (true);
        }
        leaderb.GetComponent<Image> ().enabled = true;
    }

    IEnumerator WaitDisableTransition (float waitTime) {
        yield return new WaitForSeconds (waitTime);
        DisableTransition ();
    }

    /// <summary>
    /// menu displayng helper
    /// </summary>
    void DisableTransition () {
        isInTransition = false;
    }

}