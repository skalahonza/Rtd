using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Mechanics
{
    public static class TargetingMechanis
    {
        public static Vector3 CalculateAimVector(Transform enemy, Vector3 shooterPosition, float projectileSpeed)
        {
            var rb = enemy.GetComponent<Rigidbody>();
            var velocity = new Vector3(0, 0, 0);
            if (rb != null)
            {
                velocity = rb.velocity;
            }

            var direct = (enemy.position - shooterPosition + velocity).normalized * projectileSpeed;
            // add velocity, if the angle between them is not too small
            float scalar = direct.x * velocity.x + direct.y * velocity.y + direct.z * velocity.z;
            if (Math.Acos(scalar / (direct.magnitude + velocity.magnitude)) > 30*Math.PI / 180)
            {
                direct += velocity;
            }

            return direct;
        }
    }
}