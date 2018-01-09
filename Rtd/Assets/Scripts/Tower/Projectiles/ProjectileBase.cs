using Assets.Mechanics;
using Assets.Scripts.Constants;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBase : MonoBehaviour, IDamageDealer,IPrefable
{
    public float Speed;
    public float Damage;

    /// <summary>
    /// Every projectile should return it's own prefab from resources
    /// </summary>
    /// <returns>Prefab from resources</returns>
    public abstract GameObject GetPrefab();

    public GameObject Owner;

    /// <summary>
    /// Triggers when hit with other collider, except tower colliders. If the object hit by the projectile is damagable, the damage will be dealt.
    /// </summary>
    /// <param name="other">Collider that was hit</param>
    public virtual void OnHit(Collider other)
    {   
        SoundMechanics.SpawnSound("projectile_on_hit");
        var particle = AnimationMechanics.SpawnParticle("energy_explosion");
        particle.transform.position = transform.position;
        Destroy(gameObject);
    }

    public virtual void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        // ignore tower and muzzle, and projectile owner, hit other stuff
        if (!other.CompareTag(GameTag.Tower.ToString()) 
            && !other.CompareTag(GameTag.Muzzle.ToString())
            && !other.CompareTag(GameTag.Barrier.ToString())
            && other.gameObject != Owner
            && other.gameObject.GetComponent<ProjectileTower>() == null
            )
            OnHit(other);
    }

    public virtual void DealDamage(CarSpirit car)
    {
        car.Hp -= Damage;
    }
}