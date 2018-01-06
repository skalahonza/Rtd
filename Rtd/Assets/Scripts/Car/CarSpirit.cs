using Assets.Mechanics;
using Assets.Scripts.Constants;
using Assets.Scripts.Powerups;
using Assets.Scripts.Powerups.Nitros;
using Assets.Scripts.Powerups.Projectiles;
using Assets.Scripts.Powerups.Shields;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

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

    private readonly PowerupGenerator _powerupGenerator = new PowerupGenerator(
/*        new List<Type>()
        {
            //typeof(MinePowerup),
            //typeof(SurgePowerUp),
            //typeof(MissilePowerup),
            //typeof(ReverseMissilePowerup),
            typeof(NormalShieldPowerUp),
            typeof(PaybackShieldPowerUp),
        }*/
        );
    private float _powerupSpawnPeriod = 0.0f;
    private float _shieldDisablePeriod = 0.0f;
    private float _nitroDisablePeriod = 0.0f;

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
                Debug.Log("Turning off shield" + Shield);
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

    private void SpawnPowerup()
    {
        _powerUp = gameObject.GetComponent(_powerupGenerator.GetPowerUpType()) as PowerUpBase;
        Debug.Log("Power up spawned " + _powerUp);
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
            if(isLocalPlayer)
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
        Debug.Log("Power up spawned " + _powerUp);
    }

    [ClientRpc]
    public void RpcUsePowerUp()
    {
        UsePowerUp();
    }

    public void UsePowerUp()
    {
        if (_powerUp != null)
        {
            Debug.Log("Using powerup: " + _powerUp);
            //if (gameObject.GetComponent<NetworkPlayer>() != null ? _powerUp.UseNetwork(this) : _powerUp.Use(this))
            if (_powerUp.Use(this))
            {
                //TODO Clear powerup upon successfull action
                _powerUp = null;
                _powerupSpawnPeriod = 0;
            }
            else
            {
                if (isLocalPlayer)
                    SoundMechanics.SpawnSound("error_sound");
            }
        }
        else
        {
            if (isLocalPlayer)
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