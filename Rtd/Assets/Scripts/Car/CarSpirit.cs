using Assets.Scripts.Powerups;
using UnityEngine;

public class CarSpirit : MonoBehaviour, IDamagable
{

    public float Hp;
    public float MaxMotorTorque;
    public float MaxSteeringAngle;

    public ProjectilePowerupBase PowerupBase;

    void Start()
    {
        
    }

    void Update()
    {
        if (PowerupBase == null)
            return;

        PowerupBase.UpdatePowerup(this);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PowerupBase.Use();
        }
    }

    public void SufferDamage(IDamageDealer damager)
    {
        // send my spirit to the damager and let the damager to deal me damage
        //TODO prevent if shielded
        damager.DealDamage(this);


        if (Hp <= 0)
            Destroy(gameObject);
    }
}