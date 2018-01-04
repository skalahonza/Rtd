using System.Collections.Generic;
using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups
{
    public class MinePowerup:IPowerup
    {
        private  MineBase Mine = new CarMine();

        public bool Use(CarSpirit car)
        {
            var postion = car.gameObject.transform.position + car.gameObject.transform.forward*-1 * NumberConstants.SpawningDiretionMultiplier*5;
            postion = NumberConstants.MineSpawnHeight(postion);
            var sound = SoundMechanics.SpawnSound("car_mine_sound");
            NetworkServer.Spawn(sound);
            var mine = GameObject.Instantiate(Mine.GetPrefab(), postion, new Quaternion());
            NetworkServer.Spawn(mine);
            return true;
        }

        public bool UseNetwork(CarSpirit car){
            return PowerupNetworkSpawner.spawn(this, cars);
        }

        public void UpdatePowerup(CarSpirit car)
        {
        }

        public Sprite GetPowerupIcon()
        {
            return ImageMechanics.LoadSprite("mine");
        }        
    }
}