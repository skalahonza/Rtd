using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Constants;
using Assets.Scripts.Tower.Projectiles;

public class ProjectileTower : TowerBase
{
    public Transform MuzzlePosition;
    public ProjectileBase TowerProjectile;
    protected Transform _enemy;

    public virtual void Update()
    {
        _timer += Time.deltaTime;

        // FIRE
        if (_timer >= TimeBetweenAttack && _enemiesInRange.Any())
        {
            _enemy = GetClosestEnemy(_enemiesInRange);
            Attack();
        }
    }

    /// <summary>
    /// Shoot projectile on the closest harget
    /// </summary>
    public override void Attack()
    {
        // Weapon delay reset
        _timer = 0f;

        //spawn and fire projectile
        var projectile =  Instantiate(TowerProjectile.GetPrefab(), MuzzlePosition.position, CalculateAimRotation(_enemy.position));
        projectile.GetComponent<Rigidbody>().velocity =
            (_enemy.position - projectile.transform.position).normalized * TowerProjectile.Speed;
        // TODO audio.Play();
    }

    protected Quaternion CalculateAimRotation(Vector3 targetPosition)
    {
        // Aim at the targetPosition
        // TargetPosition substracting towerPosition creates a vector pointing from the tower to the targetPosition. 
        var aimPoint = new Vector3(targetPosition.x, 0, targetPosition.z) - transform.position;
        return Quaternion.LookRotation(aimPoint);
    }    

    /// <summary>
    /// Get closest enemy from collection of enemies
    /// </summary>
    /// <param name="enemies">Collection of enemies</param>
    /// <returns>Closest enemy</returns>
    public virtual Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform target = null;
        float closestDistanceSqr = Mathf.Infinity;
        var currentPosition = transform.position;

        foreach (var potentialTarget in enemies)
        {
            var directionTotarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionTotarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                target = potentialTarget;
            }
        }
        
        return target;
    }
}

[RequireComponent(typeof(CapsuleCollider))]
public abstract class TowerBase : MonoBehaviour
{
    public float TimeBetweenAttack;
    public int AttackDamage;
    public float Radius;

    protected float _timer;

    protected readonly List<Transform> _enemiesInRange = new List<Transform>();    

    public abstract void Attack();

    public void Start()
    {
        var colider = GetComponent<CapsuleCollider>();
        colider.isTrigger = true;
        colider.radius = Radius;
        colider.height = 5;
    }

    /// <summary>
    /// When object enters the tower radius
    /// </summary>
    /// <param name="other">Other object</param>
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameTag.Player.ToString())
        {
            // Add object which enters collider to List
            _enemiesInRange.Add(other.transform);
        }
    }

    /// <summary>
    /// When game object leaves tower radius
    /// </summary>
    /// <param name="other">Other object</param>
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GameTag.Player.ToString())
        {
            // Remove object which leaves the collider from List
            _enemiesInRange.Remove(other.transform);
        }
    }
}