﻿using System.Linq;
using Assets.Scripts.Constants;
using UnityEngine;

public class MineTower : TowerBase
{
    public MineBase Mine;

    public override void Attack()
    {
        var damagable = Physics.OverlapSphere(gameObject.transform.position, Radius)
            .Select(x => x.GetComponent<CarSpirit>())
            .Where(x => x != null);

        foreach (var spirit in damagable)
        {
            var v = spirit.GetComponent<Rigidbody>().velocity;
            var spawnPosition = spirit.transform.position + NumberConstants.SpawningDiretionMultiplier * v;
            spawnPosition = NumberConstants.MineSpawnHeight(spawnPosition);
            //spawn mine
            Instantiate(Mine.GetPrefab(), spawnPosition, new Quaternion());
        }
    }
}