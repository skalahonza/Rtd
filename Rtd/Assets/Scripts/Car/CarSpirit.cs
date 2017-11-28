using Assets.Mechanics;
using Assets.Scripts.Powerups;
using UnityEngine;

public class CarSpirit : MonoBehaviour, IDamagable
{

    public float Hp;
    public float MaxMotorTorque;
    public float MaxSteeringAngle;

    [SerializeField]
    private  ProjectilePowerupBase _powerUp = new MissilePowerup();

    void Update()
    {

        //TODO REMOVE THIS
        if (_powerUp == null)
        {
            _powerUp = new MissilePowerup();
        }

        if (_powerUp == null)
            return;

        // update powerup, retarget cars etc
        _powerUp.UpdatePowerup(this);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // shoooooooooot
            // TODO audio.Play();
            _powerUp.Use(this);
        }
    }

    /// <summary>
    /// When hit by projectile or another damage dealer
    /// </summary>
    /// <param name="damager">Damager that hit me</param>
    public void SufferDamage(IDamageDealer damager)
    {
        // send my spirit to the damager and let the damager to deal me damage
        //TODO prevent if shielded
        damager.DealDamage(this);


        if (Hp <= 0)
            Destroy(gameObject);
    }
}