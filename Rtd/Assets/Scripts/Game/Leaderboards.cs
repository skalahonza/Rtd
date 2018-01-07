using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboards : MonoBehaviour {

	public List<Player> players = new List<Player>(); 

	public void OnTriggerEnter(Collider other) {
		Player oth = other.gameObject.GetComponent<Player>();
		if(oth != null){
			Debug.Log(string.Format("finish {0} {1}", players.Contains(oth),oth.checkpointOffest ));
			if(!players.Contains(oth) &&  oth.checkpointOffest != 0){
				players.Add(oth);
				oth.Finish();
			}
		}
	}
}
