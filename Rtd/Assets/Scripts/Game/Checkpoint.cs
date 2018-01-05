using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public GameObject[] positions;
    public int offset; 

     void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<Player>() == null)
            return;
        other.gameObject.GetComponent<Player>().latest = this;
        other.gameObject.GetComponent<Player>().checkpointOffest = offset;
    }
}