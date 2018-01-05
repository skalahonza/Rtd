using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups
{
    public class PowerupNetworkSpawner : NetworkBehaviour {

        public static bool spawn(IPowerup pwup, CarSpirit car){
            PowerupNetworkSpawner inst = new PowerupNetworkSpawner();
            bool ret = pwup.Spawnable();
            if(ret){
                GameObject.Find("network").GetComponent<PowerupNetworkSpawner>().CmdServerSpawn((MinePowerup)pwup, car.gameObject.transform.position, car.gameObject.transform.forward);
            }
            return ret;
        }

        [Command]
        public void CmdServerSpawn(MinePowerup pwup, Vector3 pos, Vector3 forward){
            Debug.Log("mine spawned");
            pwup.Use(pos, forward);
        }

    }
}