using UnityEngine;
/// <summary>
/// Destroy gameobject after audio playing is finished
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var audioSource = GetComponent<AudioSource>();
        Destroy(gameObject,audioSource.clip.length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}