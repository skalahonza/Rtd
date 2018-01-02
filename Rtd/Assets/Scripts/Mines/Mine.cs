using Assets.Mechanics;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class MineBase : MonoBehaviour,IDamageDealer,IPrefable
{
    public float Damage = 50f;
    public virtual void Start()
    {
        var sphere = GetComponent<SphereCollider>();
        sphere.isTrigger = true;
    }

    void Update()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        var damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            //Expllosion
            var sound = SoundMechanics.SpawnSound("projectile_on_hit");
            var particle = AnimationMechanics.SpawnParticle("energy_explosion");
            particle.transform.position = gameObject.transform.position;
            sound.gameObject.transform.position = gameObject.transform.position;

            damagable.SufferDamage(this);
            Destroy(gameObject);
        }
    }

    public void DealDamage(CarSpirit car)
    {
        car.Hp -= Damage;
    }

    public abstract GameObject GetPrefab();
}