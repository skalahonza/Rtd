using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {
    public GameObject prefab;
    public Color[] colors = new Color[5];
    public GameObject go;

	private void Update() {
        Leaderboards ld = GameObject.Find("lead").GetComponent<Leaderboards>();
        if(ld.players.Count != go.transform.childCount){
            Render(ld);
        }
    }

    private void Render(Leaderboards ld){
        for(int i = 0; i < go.transform.childCount; i++)
        {
            Destroy(go.transform.GetChild(i).gameObject);
        }
        int d =1;
        foreach (var item in ld.players)
        {
            Text x = Instantiate(prefab, go.transform).GetComponent<Text>();
            x.text = d +": <color=#" + ColorUtility.ToHtmlStringRGBA (colors[item.cid]) + ">" + item.cname + "</color>";
            d++;
        }
    }
}
