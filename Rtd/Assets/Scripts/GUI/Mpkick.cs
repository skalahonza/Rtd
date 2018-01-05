using UnityEngine;
using UnityEngine.UI;

public class Mpkick : MonoBehaviour {
    public void UpdateDD(Button dd){
        GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>().Kick(dd);
    }
}