using System;
using UnityEngine;

namespace Assets.Mechanics
{
    public static class SoundMechanics
    {
        private const string dir = "Sounds";
        public static GameObject SpawnSound(string soundName)
        {            
            var prefab = Resources.Load<GameObject>(dir + "\\" + soundName);
            return GameObject.Instantiate(prefab);
        }

        public static GameObject SpawnSound(string soundName, Transform parent)
        {
            var prefab = Resources.Load<GameObject>(dir + "\\" + soundName);
            return GameObject.Instantiate(prefab,parent);
        }
    }
}