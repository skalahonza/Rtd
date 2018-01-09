using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour {
    public GameObject prefab;
    public GameObject button;

    public Color[] colors = new Color[5];
    public GameObject go;

    private int fin =0;

	private void Update() {
        Leaderboards ld = GameObject.FindObjectOfType<Leaderboards>();
        if(ld.players.Count != go.transform.childCount){
            Render(ld);
            if(Assets.Mechanics.MultiplayerHelper.IsMultiplayer())
            CheckButton();
        }
    }

    private void Render(Leaderboards ld){
        for(int i = 0; i < go.transform.childCount; i++)
        {
            Destroy(go.transform.GetChild(i).gameObject);
        }
        fin = 0;
        int d =1;
        foreach (var item in ld.players)
        {
            fin++;
            Text x = Instantiate(prefab, go.transform).GetComponent<Text>();
            x.text = d +": <color=#" + ColorUtility.ToHtmlStringRGBA (colors[item.cid]) + ">" + item.cname + "</color>";
            d++;
        }
    }

    public void ReturnBtt(){
        Debug.Log(string.Format("Game finished MP{0}",Assets.Mechanics.MultiplayerHelper.IsMultiplayer()));
        if(Assets.Mechanics.MultiplayerHelper.IsMultiplayer()){
            GameObject.FindObjectOfType<Lobby>().SendReturnToLobby();
        }else{
            SceneManager.LoadScene(4);
        }
    }

    private void CheckButton(){
        Debug.Log(string.Format("Finished {0} of {1}", fin,GameObject.FindObjectsOfType<CarSpirit>().Length ));
        if(GameObject.FindObjectsOfType<CarSpirit>().Length == fin)
            button.SetActive(true);
    }    
}
