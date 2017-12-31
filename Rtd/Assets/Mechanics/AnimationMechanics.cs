using UnityEngine;

namespace Assets.Mechanics
{
    public static class AnimationMechanics
    {
        public static GameObject SpawnParticle(string name)
        {
            const string dir = "Particles";
            var prefab = Resources.Load<GameObject>(dir + "\\" + name);
            var particle = GameObject.Instantiate(prefab);
            particle.AddComponent<ParticlePlayer>();
            return particle;
        }

        public static GameObject SpawnParticle(string name, Transform parent)
        {
            var particle = SpawnParticle(name);
            particle.transform.parent = parent;
            return particle;
        }
    }
}