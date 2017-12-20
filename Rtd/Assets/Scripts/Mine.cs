using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Mine : MonoBehaviour,IDamageDealer
{
    public float Damage = 50f;
    public float Reach = 25f;
    void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    void Update()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        var damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.SufferDamage(this);
            Destroy(gameObject);
        }
    }

    public void DealDamage(CarSpirit car)
    {
        car.Hp -= Damage;
    }
}
