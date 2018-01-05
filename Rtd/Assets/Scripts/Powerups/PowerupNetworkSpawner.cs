using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts.Constants;
using Assets.Mechanics;

namespace Assets.Scripts.Powerups
{
    public class PowerupNetworkSpawner : NetworkBehaviour {

        public static bool spawn(IPowerup pwup, CarSpirit car){
            bool ret = pwup.Spawnable();
            if(ret){
                GameObject.Find("GameObject").GetComponent<PowerupNetworkSpawner>().CmdServerSpawn(car.gameObject.transform.position, car.gameObject.transform.forward);
            }
            return ret;
        }

        [Command]
        public void CmdServerSpawn(Vector3 pos, Vector3 forward){
            MineBase Mine = new CarMine();
            var postion = pos + forward*-1 * NumberConstants.SpawningDiretionMultiplier*5;
            postion = NumberConstants.MineSpawnHeight(postion);
            var sound = SoundMechanics.SpawnSound("car_mine_sound");
            NetworkServer.Spawn(sound);
            var mine = GameObject.Instantiate(Mine.GetPrefab(), postion, new Quaternion());
            NetworkServer.Spawn(mine);
        }

    }
}