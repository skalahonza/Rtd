using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups
{
    public class PowerupNetworkSpawner : NetworkBehaviour {

        public static bool spawn(IPowerup pwup, CarSpirit car){
            PowerupNetworkSpawner inst = new PowerupNetworkSpawner();
            bool ret = pwup.Spawnable();
            if(ret){
                inst.CmdServerSpawn((MinePowerup)pwup, car.gameObject.transform);
            }
            return ret;
        }

        [Command]
        public void CmdServerSpawn(MinePowerup pwup, Transform car){
            pwup.Use(car);
        }

    }
}