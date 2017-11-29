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

    public  void OnTriggerEnter(Collider other)
    {
        // ignore tower and muzzle, hit other stuff
        if (!other.CompareTag(GameTag.Tower.ToString()) && !other.CompareTag(GameTag.Muzzle.ToString()))
            OnHit(other);
    }


    public virtual void DealDamage(CarSpirit car)
    {
        car.HP -= Damage;
    }
}