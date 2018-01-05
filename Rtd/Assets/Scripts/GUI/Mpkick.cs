using UnityEngine;
using UnityEngine.UI;

public class Mpkick : MonoBehaviour {
    public void UpdateDD(Button dd){
        GameObject.Find("network").GetComponent<LobbyController>().Kick(dd);
    }
}