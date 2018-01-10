using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Multiplayer select car type dropdown handler
/// </summary>
public class Mpdropdown : MonoBehaviour {

    /// <summary>
    /// changevalue event
    /// </summary>
    /// <param name="dd">
    /// dropdown
    /// </param>
    public void UpdateDD(Dropdown dd){
        GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>().PlayerDropdownChange(dd);
    }
}