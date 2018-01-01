using UnityEngine;

namespace Assets.Mechanics
{
    public static class AnimationMechanics
    {
        private static GameObject PrepareParticlePrefab(string name)
        {
            const string dir = "Particles";
            var prefab = Resources.Load<GameObject>(dir + "\\" + name);
            return prefab;
        }
        public static GameObject SpawnParticle(string name)
        {
            var prefab = PrepareParticlePrefab(name);
            var particle = GameObject.Instantiate(prefab);
            particle.AddComponent<ParticlePlayer>();
            return particle;
        }

        public static GameObject SpawnParticle(string name, Transform parent)
        {
            var prefab = PrepareParticlePrefab(name);
            var particle = GameObject.Instantiate(prefab,parent);
            particle.AddComponent<ParticlePlayer>();
            return particle;
        }
    }
}