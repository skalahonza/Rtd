using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups
{
    public class PowerupNetworkSpawner : NetworkBehaviour {

        public static bool spawn(IPowerup pwup, CarSpirit car){
            PowerupNetworkSpawner inst = new PowerupNetworkSpawner();
            return inst.serverSpawn(pwup, car);
        }

        [Command]
        public bool serverSpawn(IPowerup pwup, CarSpirit car){
            return pwup.Use(car);
        }

    }
}