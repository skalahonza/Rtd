﻿using Assets.Mechanics;
using Assets.Scripts.Constants;
using Assets.Scripts.Powerups;
using Assets.Scripts.Powerups.Nitros;
using Assets.Scripts.Powerups.Projectiles;
using Assets.Scripts.Powerups.Shields;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

/// <summary>
/// Holds basic information about car
/// </summary>
[RequireComponent(typeof(MissilePowerup))]
[RequireComponent(typeof(ReverseMissilePowerup))]
[RequireComponent(typeof(MinePowerup))]
[RequireComponent(typeof(SurgePowerUp))]
[RequireComponent(typeof(NormalShieldPowerUp))]
[RequireComponent(typeof(PaybackShieldPowerUp))]
[RequireComponent(typeof(SpeedyNitrPowerUp))]
[RequireComponent(typeof(TimedNitroPowerUp))]
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

    private readonly PowerupGenerator _powerupGenerator = new PowerupGenerator();
    private float _powerupSpawnPeriod;
    private float _shieldDisablePeriod;
    private float _nitroDisablePeriod;

    public ShieldBase Shield;
    public NitroBase Nitro;

    void Update()
    {
        //increment powerup time if no powerup present
        if (_powerUp == null)
            _powerupSpawnPeriod += Time.deltaTime;

        if (_powerupSpawnPeriod > NumberConstants.PowerUpSpawn)
        {
            //Spawn powerup after time limit
            if (_powerUp == null)
            {
                if (MultiplayerHelper.IsMultiplayer())
                    CmdSpawnPowerup();
                else
                {
                    SpawnPowerup();
                }
            }

            _powerupSpawnPeriod = 0;
        }

        //clear shield after warmoff time
        if (Shield != null)
        {
            _shieldDisablePeriod += Time.deltaTime;
            if (_shieldDisablePeriod > Shield.Duration.TotalSeconds)
            {
                Shield.Clean(this);
                Shield = null;
                _shieldDisablePeriod = 0;
            }
        }

        //clear nitro after warmoff time
        if (Nitro != null)
        {
            _nitroDisablePeriod += Time.deltaTime;
            if (_nitroDisablePeriod > Nitro.Time)
            {
                _nitroDisablePeriod = 0;
                Nitro.Clean(this);
                Nitro = null;
            }
        }

        // update powerup, retarget cars etc
        if (_powerUp != null)
            _powerUp.UpdatePowerup(this);
    }

    private void SpawnPowerup()
    {
        _powerUp = gameObject.GetComponent(_powerupGenerator.GetPowerUpType()) as PowerUpBase;
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
            if(isLocalPlayer || !MultiplayerHelper.IsMultiplayer())
                gameObject.GetComponent<Player>().Respawn();
        }
    }

    /// <summary>
    /// Use currentely weared powerup
    /// </summary>
    [Command]
    public void CmdUsePowerUp()
    {
        RpcUsePowerUp();   
    }

    [Command]
    private void CmdSpawnPowerup()
    {
        RpcSpawnPowerup(new Random().Next(100));
    }

    [ClientRpc]
    private void RpcSpawnPowerup(int rand)
    {
        SpawnPowerup(rand);
    }

    private void SpawnPowerup(int rand)
    {
        _powerUp = gameObject.GetComponent(_powerupGenerator.GetPowerUpType(rand)) as PowerUpBase;
    }

    /// <summary>
    /// Send rpc message that this car is using a powerup
    /// </summary>
    [ClientRpc]
    public void RpcUsePowerUp()
    {
        UsePowerUp();
    }

    /// <summary>
    /// Use current powerup
    /// </summary>
    public void UsePowerUp()
    {
        if (_powerUp != null)
        {
            //if (gameObject.GetComponent<NetworkPlayer>() != null ? _powerUp.UseNetwork(this) : _powerUp.Use(this))
            if (_powerUp.Use(this))
            {
                //TODO Clear powerup upon successfull action
                _powerUp = null;
                _powerupSpawnPeriod = 0;
            }
            else
            {
                if (isLocalPlayer || !MultiplayerHelper.IsMultiplayer())
                    if (gameObject.GetComponent<AIPlayer>() == null)
                        SoundMechanics.SpawnSound("error_sound");
            }
        }
        else
        {
            if (isLocalPlayer || !MultiplayerHelper.IsMultiplayer())
                if(gameObject.GetComponent<AIPlayer>() == null)
                    SoundMechanics.SpawnSound("error_sound");
        }
    }

    /// <summary>
    /// When hit by surge - your powerup will be removed and you need to wait for another one
    /// </summary>
    public void SilencePowerup()
    {
        _powerUp = null;
        _powerupSpawnPeriod = 0;
    }
}