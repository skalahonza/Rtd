﻿using UnityEngine;

namespace Assets.Scripts.Constants
{
    public static class NumberConstants
    {
        /// <summary>
        /// Angle for targeting mechanics, while aiming a projectile powerup
        /// </summary>
        public const float DetetionAngle = 30f;

        /// <summary>
        /// Number of seconds between each powerup spawn
        /// </summary>
        public const float PowerUpSpawn = 5f;

        /// <summary>
        /// Multiply direction vector of moving cars and spawn mine on this position
        /// </summary>
        public const float SpawningDiretionMultiplier = 2.5f;

        /// <summary>
        /// Tower mine will vanish after this time pass
        /// </summary>
        public const float TowerMineVanishTime = 7.5f;

        public static Vector3 MineSpawnHeight(Vector3 spawnPosition)
        {
            spawnPosition.y += 1;
            return spawnPosition;
        }

        public const int NormalShieldSeconds = 50;

        public const int PaybackShieldSeconds = 25;
    }
}