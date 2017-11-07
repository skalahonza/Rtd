using UnityEngine;

public class CarSpirit : MonoBehaviour, IDamagable
{

    public float HP;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    public void SufferDamage(IDamageDealer damager)
    {
        // send my spirit to the damager and let the damager to deal me damage
        damager.DealDamage(this);


        if (HP <= 0)
            Destroy(gameObject);
    }
}