using Assets.Scripts.Constants;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class ProjectileBase : MonoBehaviour, IDamageDealer
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
        var parent = other.transform.gameObject;
        IDamagable target;
        if ((target = parent.GetComponent<IDamagable>()) != null)
        {
            target.SufferDamage(this);
        }

        Destroy(gameObject);
    }

    public virtual void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        // ignore tower and muzzle, and projectile owner, hit other stuff
        if (!other.CompareTag(GameTag.Tower.ToString()) 
            && !other.CompareTag(GameTag.Muzzle.ToString())
            && other.gameObject != Owner
            )
            OnHit(other);
    }

    public virtual void DealDamage(CarSpirit car)
    {
        car.Hp -= Damage;
    }
}