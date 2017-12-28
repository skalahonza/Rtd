using UnityEngine;

namespace Assets.Mechanics
{
    public static class SoundMechanics
    {
        public static GameObject SpawnSound(string soundName)
        {
            const string dir = "Sounds";
            var prefab = Resources.Load<GameObject>(dir + "\\" + soundName);
            return GameObject.Instantiate(prefab);
        }
    }
}