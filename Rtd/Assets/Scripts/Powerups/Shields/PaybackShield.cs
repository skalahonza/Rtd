﻿using System;
using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Powerups.Shields
{
    public class PaybackShield : ShieldBase {
        public override bool ResolveHit(IDamageDealer dealer, CarSpirit car)
        {
            var projectile = dealer as ProjectileBase;
            if (projectile != null)
            {
                var target = projectile.Owner;
                if (target != null)
                {
                    var velocity = TargetingMechanis.CalculateAimVelocityVector(target.transform,
                        car.gameObject.transform.position, projectile.Speed);
                    var position = projectile.transform.position;
                    var prefab = projectile.GetPrefab();
                    var newProjectile = GameObject.Instantiate(prefab,
                        position,
                        TargetingMechanis.CalculateAimRotation(target.transform.position, car.transform.position)
                    );

                    var projBase = newProjectile.GetComponent<ProjectileBase>();
                    projBase.Owner = car.gameObject;

                    newProjectile.GetComponent<Rigidbody>().velocity = velocity;
                }
            }
            return false;
        }

        public override TimeSpan Duration
        {
            get { return TimeSpan.FromSeconds(NumberConstants.PaybackShieldSeconds); }
        }

        public override void Apply(CarSpirit car)
        {
            var sound = SoundMechanics.SpawnSound("short_shield_sound");
            sound.transform.parent = car.transform;
            shieldSoundPlayer = sound;
            base.Apply(car);
        }

        protected override GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("Shields\\shield_emerald");
        }
    }
}