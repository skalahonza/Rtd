using System;
using UnityEngine;

namespace Assets.Mechanics
{
    public static class SoundMechanics
    {
        private const string dir = "Sounds";

        /// <summary>
        /// Spawn sound gameobject
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <returns>Instance of the sound</returns>
        public static GameObject SpawnSound(string soundName)
        {            
            var prefab = Resources.Load<GameObject>(dir + "\\" + soundName);
            return GameObject.Instantiate(prefab);
        }

        /// <summary>
        /// Spawn sound gameobject
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="parent">Holder of the sound</param>
        /// <returns>Instance of the sound</returns>
        public static GameObject SpawnSound(string soundName, Transform parent)
        {
            var prefab = Resources.Load<GameObject>(dir + "\\" + soundName);
            return GameObject.Instantiate(prefab,parent);
        }
    }
}