using System.Collections.Generic;
using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class MinePowerup:IPowerup
    {
        private  MineBase Mine = new CarMine();
        public List<GameObject> ObjectsToSynchronize { get; private set; }

        public MinePowerup()
        {
            ObjectsToSynchronize = new List<GameObject>();
        }

        public bool Use(CarSpirit car)
        {
            var postion = car.gameObject.transform.position + car.gameObject.transform.forward*-1 * NumberConstants.SpawningDiretionMultiplier*5;
            postion = NumberConstants.MineSpawnHeight(postion);
            var sound = SoundMechanics.SpawnSound("car_mine_sound");
            ObjectsToSynchronize.Add(sound);
            var mine = GameObject.Instantiate(Mine.GetPrefab(), postion, new Quaternion());
            ObjectsToSynchronize.Add(mine);
            return true;
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