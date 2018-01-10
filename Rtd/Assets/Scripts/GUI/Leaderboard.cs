using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// class handling leaderboards functions
/// </summary>
public class Leaderboard : MonoBehaviour {
    public GameObject prefab;
    public GameObject button;

    public Color[] colors = new Color[5];
    public GameObject go;

    private int fin;

    /// <summary>
    /// check if re-rendering is neccessary
    /// </summary>
    private void Update () {
        Leaderboards ld = FindObjectOfType<Leaderboards> ();
        if (ld.players.Count != go.transform.childCount) {
            Render (ld);
            if (Assets.Mechanics.MultiplayerHelper.IsMultiplayer ())
                CheckButton ();
        }
    }

    /// <summary>
    /// RE-render leaderboards
    /// </summary>
    /// <param name="ld">
    /// leaderboards data
    /// </param>
    private void Render (Leaderboards ld) {
        for (int i = 0; i < go.transform.childCount; i++) {
            Destroy (go.transform.GetChild (i).gameObject);
        }
        fin = 0;
        int d = 1;
        foreach (var item in ld.players) {
            fin++;
            Text x = Instantiate (prefab, go.transform).GetComponent<Text> ();
            x.text = d + ": <color=#" + ColorUtility.ToHtmlStringRGBA (colors[item.cid]) + ">" + item.cname + "</color>";
            d++;
        }
    }

    /// <summary>
    /// return button click event
    /// </summary>
    public void ReturnBtt () {
        Debug.Log (string.Format ("Game finished MP{0}", Assets.Mechanics.MultiplayerHelper.IsMultiplayer ()));
        if (Assets.Mechanics.MultiplayerHelper.IsMultiplayer ()) {
            FindObjectOfType<Lobby> ().SendReturnToLobby ();
        } else {
            SceneManager.LoadScene (4);
        }
    }

    /// <summary>
    /// multiplayer - check and show button when everybody finished track
    /// </summary>
    private void CheckButton () {
        Debug.Log (string.Format ("Finished {0} of {1}", fin, FindObjectsOfType<CarSpirit> ().Length));
        if (FindObjectsOfType<CarSpirit> ().Length == fin)
            button.SetActive (true);
    }
}