using Assets.Scripts.Constants;
using Assets.Scripts.Powerups;
using UnityEngine;

public class CarSpirit : MonoBehaviour, IDamagable
{
    public float Hp;
    public float MaxMotorTorque;
    public float MaxSteeringAngle;

    [SerializeField]
    private IPowerup _powerUp = new SurgePowerUp();
    private PowerupGenerator powerupGenerator = new PowerupGenerator();
    private float period = 0.0f;

    void Update()
    {
        period += Time.deltaTime;
        if (period > NumberConstants.PowerUpSpawn)
        {
            //Do Stuff
            if (_powerUp == null)
                _powerUp = powerupGenerator.GetPowerUp();

            period = 0;
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
        // send my spirit to the damager and let the damager to deal me damage
        //TODO prevent if shielded
        damager.DealDamage(this);


        if (Hp <= 0)
            Destroy(gameObject);
    }

    public void UsePowerUp()
    {
        if (_powerUp != null && _powerUp.Use(this))
        {
            //TODO clear powerup upon successfull action
            _powerUp = null;
        }
    }
}