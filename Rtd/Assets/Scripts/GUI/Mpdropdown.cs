using UnityEngine;
using UnityEngine.UI;

public class Mpdropdown : MonoBehaviour {
    public void UpdateDD(Dropdown dd){
        GameObject.Find("network").GetComponent<LobbyController>().PlayerDropdownChange(dd);
    }
}