using UnityEngine;

namespace Assets.Mechanics
{
    public static class SoundMechanics
    {
        public static void SpawnSound(string soundName)
        {
            const string dir = "Sounds";
            var prefab = Resources.Load<GameObject>(dir + "\\" + soundName);
            GameObject.Instantiate(prefab);
        }
    }
}