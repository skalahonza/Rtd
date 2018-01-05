using UnityEngine;
using UnityEngine.UI;

public class Mpdropdown : MonoBehaviour {
    public void UpdateDD(Dropdown dd){
        GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>().PlayerDropdownChange(dd);
    }
}