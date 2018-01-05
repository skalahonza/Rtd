using System;
using System.Collections.Generic;
using Assets.Mechanics;
using Assets.Scripts.Constants;
using Assets.Scripts.Powerups;
using Assets.Scripts.Powerups.Nitros;
using Assets.Scripts.Powerups.Shields;
using UnityEngine;
using UnityEngine.Networking;

public class CarSpirit : NetworkBehaviour, IDamagable
{
    public float MaxHp;
    public float Hp;
    public float MaxMotorTorque;
    public float MaxSteeringAngle;
    public float maxRPM = 1000f;
    public float maxSpeed = 207;
    public float maxReverseSpeed = 75;

    public PowerUpBase _powerUp;

    private readonly PowerupGenerator _powerupGenerator = new PowerupGenerator(
        new List<Type>()
        {
            typeof(MinePowerup)
        }
        );
    private float _powerupSpawnPeriod = 0.0f;
    private float _shieldDisablePeriod = 0.0f;
    private float _nitroDisablePeriod = 0.0f;
    public ShieldBase Shield;
    public NitroBase Nitro;

    void Update()
    {
        _powerupSpawnPeriod += Time.deltaTime;

        if (_powerupSpawnPeriod > NumberConstants.PowerUpSpawn)
        {
            //Do Stuff
            if (_powerUp == null)
            {
                gameObject.AddComponent(_powerupGenerator.GetPowerUpType());
                _powerUp = gameObject.GetComponent<PowerUpBase>();
                
                Debug.Log("Power up spawned " + _powerUp);
            }

            _powerupSpawnPeriod = 0;
        }

        if (Shield != null)
        {
            _shieldDisablePeriod += Time.deltaTime;
            if (_shieldDisablePeriod > Shield.Duration.TotalSeconds)
            {
                Debug.Log("Turning off shield" + Shield);
                Shield.Clean(this);
                Shield = null;
                _shieldDisablePeriod = 0;
            }
        }

        if (Nitro != null)
        {
            _nitroDisablePeriod += Time.deltaTime;
            if (_nitroDisablePeriod > Nitro.Time)
            {
                Debug.Log("Turning off nitro " + Nitro);
                _nitroDisablePeriod = 0;
                Nitro.Clean(this);
                Nitro = null;
            }
        }

        // update powerup, retarget cars etc
        if (_powerUp != null)
            _powerUp.UpdatePowerup(this);
    }

    /// <summary>
    /// When hit by projectile or another damage dealer
    /// </summary>
    /// <param name="damager">Damager that hit me</param>
    public void SufferDamage(IDamageDealer damager)
    {
        //Prevent damage if shielded
        if (Shield != null)
            if (!Shield.ResolveHit(damager, this))
                return;

        // send my spirit to the damager and let the damager to deal me damage
        damager.DealDamage(this);

        // check car destroyed
        if (Hp <= 0)
        {
            //TODO VISUALIZE DESTROYING waint and respawn
            gameObject.GetComponent<Player>().Respawn();
        }
    }
    
    /// <summary>
    /// Use currentely weared powerup
    /// </summary>
    public void UsePowerUp()
    {
        if (_powerUp != null)
        {
            Debug.Log("Using powerup: " + _powerUp);
            if (_powerUp.Use(this))
            {                
                //TODO Clear powerup upon successfull action
                Destroy(_powerUp);
                _powerUp = null;
                _powerupSpawnPeriod = 0;
            }
            else
            {
                SoundMechanics.SpawnSound("error_sound");
            }
        }
        else
        {
            SoundMechanics.SpawnSound("error_sound");
        }
    }
}