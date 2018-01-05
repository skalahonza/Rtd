using UnityEngine.UI;
using UnityEngine;

public class SpConfig : MonoBehaviour {
    public string scene;
    public RawImage image
    {
        get { return GameObject.Find(scene).GetComponent<RawImage>(); }
    }
}