using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class MinePowerup:IPowerup
    {
        private  MineBase Mine = new CarMine();
        public bool Use(CarSpirit car)
        {
            var postion = car.gameObject.transform.position + car.gameObject.transform.forward*-1 * NumberConstants.SpawningDiretionMultiplier*5;
            postion = NumberConstants.MineSpawnHeight(postion);
            SoundMechanics.SpawnSound("car_mine_sound");
            GameObject.Instantiate(Mine.GetPrefab(), postion, new Quaternion());
            return true;
        }

        public void UpdatePowerup(CarSpirit car)
        {
            //TODO CHECK IF THE MINE CAN BE SPAWNED
        }
    }
}