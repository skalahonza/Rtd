using Assets.Scripts.Powerups;
using UnityEngine;

public class CarSpirit : MonoBehaviour, IDamagable
{

    public float Hp;
    public float MaxMotorTorque;
    public float MaxSteeringAngle;

    [SerializeField]
    private ProjectilePowerupBase _powerUp = new MissilePowerup();

    void Update()
    {
        if (_powerUp == null)
            return;

        // update powerup, retarget cars etc
        _powerUp.UpdatePowerup(this);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_powerUp.Use(this))
            {
                //TODO clear powerup upon successfull action
            }
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