using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public GameObject[] positions;
    public int offset; 

     void OnTriggerEnter(Collider other) {
        Debug.Log("I am here\n");
        other.gameObject.GetComponent<Player>().latest = this;
        other.gameObject.GetComponent<Player>().checkpointOffest = offset;
    }
}