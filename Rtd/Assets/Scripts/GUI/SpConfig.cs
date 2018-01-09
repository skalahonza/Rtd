using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// map game data
/// </summary>
public class SpConfig : MonoBehaviour {

    public string scene;
    public RawImage image
    {
        get { return GameObject.Find(scene).GetComponent<RawImage>(); }
    }
}