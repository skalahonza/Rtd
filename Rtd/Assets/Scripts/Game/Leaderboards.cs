﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Playerx {
	public int cid;
	public string cname;
}

public class Leaderboards : MonoBehaviour {
	public List<Playerx> players = new List<Playerx>(); 

	public void OnTriggerEnter(Collider other) {
		Player oth = other.gameObject.GetComponent<Player>();
		if(oth != null){
			if(!oth.finished &&  oth.checkpointOffest != 0){
				if(Assets.Mechanics.MultiplayerHelper.IsMultiplayer()){
					other.gameObject.GetComponent<NetworkPlayer>().CmdFinished(oth.cid, oth.cname);
					oth.Finish();
				}else{
					Playerx p = new Playerx();
					p.cid = oth.cid;
					p.cname = oth.cname;
       				players.Add(p);
					oth.Finish();
				}
			}
		}
	}
}
