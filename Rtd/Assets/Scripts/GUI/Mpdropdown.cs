using UnityEngine;

public class Mpdropdown : MonoBehaviour {
    public void UpdateDD(){
        GameObject.Find("network").GetComponent<LobbyController>().PlayerDropdownChange();
    }
}