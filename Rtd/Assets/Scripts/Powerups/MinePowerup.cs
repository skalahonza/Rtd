using System.Collections.Generic;
using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Powerups
{
    public class MinePowerup: PowerUpBase
    {
        public override bool Use(CarSpirit car)
        {
            
            // SpawnMine();
            CmdFire();
            return true;
        }

        private GameObject SpawnMine()
        {
            CarSpirit car = gameObject.GetComponent<CarSpirit>();
            var postion = car.gameObject.transform.position + car.gameObject.transform.forward * -1 * NumberConstants.SpawningDiretionMultiplier * 5;
            postion = NumberConstants.MineSpawnHeight(postion);
            return Instantiate(Resources.Load<GameObject>("car_mine"), postion, new Quaternion());
        }

        [Command]
        void CmdFire()
        {
            var mine = SpawnMine();
            NetworkServer.Spawn(mine);
        }

        public override void UpdatePowerup(CarSpirit car)
        {
        }

        public override Sprite GetPowerupIcon()
        {
            return ImageMechanics.LoadSprite("mine");
        }        
    }
}