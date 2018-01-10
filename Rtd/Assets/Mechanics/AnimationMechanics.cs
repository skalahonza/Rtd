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

        /// <summary>
        /// Spawn particle effect
        /// </summary>
        /// <param name="name">Name of the particle effect</param>
        /// <returns>Instantized particle</returns>
        public static GameObject SpawnParticle(string name)
        {
            var prefab = PrepareParticlePrefab(name);
            var particle = Object.Instantiate(prefab);
            particle.AddComponent<ParticlePlayer>();
            return particle;
        }

        /// <summary>
        /// Spawn particle effect
        /// </summary>
        /// <param name="name">Name of the particle effect</param>
        /// <param name="parent">Parent transform - holder of the particle object</param>
        /// <returns>Instantized particle</returns>
        public static GameObject SpawnParticle(string name, Transform parent)
        {
            var prefab = PrepareParticlePrefab(name);
            var particle = Object.Instantiate(prefab,parent);
            particle.AddComponent<ParticlePlayer>();
            return particle;
        }
    }
}