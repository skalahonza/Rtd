﻿using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    /// <summary>
    /// Apply mine powerup on a car
    /// </summary>
    public class MinePowerup: PowerUpBase
    {
        public override bool Use(CarSpirit car)
        {            
            SpawnMine();            
            return true;
        }

        private GameObject SpawnMine()
        {
            var postion = gameObject.transform.position + gameObject.transform.forward * -1 * NumberConstants.SpawningDiretionMultiplier * 5;
            postion = NumberConstants.MineSpawnHeight(postion);
            return Instantiate(Resources.Load<GameObject>("car_mine"), postion, new Quaternion());
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