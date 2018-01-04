using UnityEngine;
using UnityEngine.Networking;

public class PoweropNetworkSpawner : NetworkBehaviour {

    public static bool spawn(IPowerup pwup, CarSpirit car){
        inst = new PoweropNetworkSpawner();
        return inst.serverSpawn(IPowerup pwup, CarSpirit car);
    }

    [Command]
    public bool serverSpawn(IPowerup pwup, CarSpirit car){
        return pwup.Use(car);
    }

}