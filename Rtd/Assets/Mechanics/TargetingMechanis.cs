using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Mechanics
{
    public static class TargetingMechanis
    {
        public static Vector3 CalculateAimVelocityVector(Transform enemy, Vector3 shooterPosition, float projectileSpeed)
        {
            var rb = enemy.GetComponent<Rigidbody>();
            var velocity = new Vector3(0, 0, 0);
            if (rb != null)
            {
                velocity = rb.velocity;
            }

            var direct = (enemy.position - shooterPosition + velocity).normalized * projectileSpeed;
            direct += velocity;

            return direct;
        }
    }
}