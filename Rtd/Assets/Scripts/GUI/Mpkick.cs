using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Multiplayer kick player button handler
/// </summary>
public class Mpkick : MonoBehaviour {

    /// <summary>
    /// click event
    /// </summary>
    /// <param name="dd">
    /// button
    /// </param>
    public void UpdateDD(Button dd){
        GameObject.FindGameObjectsWithTag("network")[0].GetComponent<LobbyController>().Kick(dd);
    }
}