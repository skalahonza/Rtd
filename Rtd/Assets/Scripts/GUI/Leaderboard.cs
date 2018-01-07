using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {
    public GameObject prefab;
    public Color[] colors = new Color[5];

	private void Update() {
        Leaderboards ld = GameObject.Find("lead").GetComponent<Leaderboards>();
        Debug.Log(string.Format("lol {0} {1}",ld.players.Count,transform.childCount ));
        if(ld.players.Count != transform.childCount){
            Render(ld);
        }
    }

    private void Render(Leaderboards ld){
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        int d =1;
        foreach (var item in ld.players)
        {
            Text x = Instantiate(prefab, transform).GetComponent<Text>();
            x.text = d +": <color=#" + ColorUtility.ToHtmlStringRGBA (colors[item.cid]) + ">" + item.cname + "</color>";
            d++;
        }
    }
}
