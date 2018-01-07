using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Playerx {
	public int cid;
	public string cname;
}

[RequireComponent(typeof(NetworkIdentity))]
public class Leaderboards : NetworkBehaviour {

	public List<Playerx> players = new List<Playerx>(); 

	public void OnTriggerEnter(Collider other) {
		Player oth = other.gameObject.GetComponent<Player>();
		if(oth != null){
			if(!oth.finished &&  oth.checkpointOffest != 0){
				if(Assets.Mechanics.MultiplayerHelper.IsMultiplayer()){
					CmdFinished(oth.cid, oth.cname);
					oth.Finish();
				}else{
					Debug.Log(string.Format("sse SINGLE"));
					Playerx p = new Playerx();
					p.cid = oth.cid;
					p.cname = oth.cname;
       				players.Add(p);
					oth.Finish();
				}
			}
		}
	}

	[Command]
	public void CmdFinished(int plid, string cname)
    {
		Debug.Log(string.Format("I have finished"));
        RpcAddFinishedPlayer(plid, cname);   
    }

	[ClientRpc]
    private void RpcAddFinishedPlayer(int plid, string cname)
    {
		Debug.Log(string.Format("sse {0} finished", cname));
		Playerx p = new Playerx();
		p.cid = plid;
		p.cname = cname;
        players.Add(p);
    }
}
