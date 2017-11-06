using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Constants;
using Assets.Scripts.Tower.Projectiles;

public class TowerTargetting : MonoBehaviour
{

    public float timeBetweenAttack = 0.5f;
    public int attackDamage = 10;
    public float turretTurnSpeed = 3.5f;
    public float errorAmount = 0f;

    public Transform turretPivot;
    public Transform muzzlePosition;
    public ProjectileBase towerProjectile;

    float aimError;
    float timer;

    private readonly List<Transform> _enemiesInRange = new List<Transform>();
    private Transform enemy;    

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

            // If there is only one item in the List set it as target
            if (_enemiesInRange.Any())
            {
                enemy = GetClosestEnemy(_enemiesInRange);
            }
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

            // If the List has objects in it set the closest as target
            if (_enemiesInRange.Any())
            {
                // TO-DO Turret turn delay
                enemy = GetClosestEnemy(_enemiesInRange);
            }
            else
            {
                enemy = null;
                _enemiesInRange.Clear();
            }
        }
    }


    public virtual void Update()
    {
        timer += Time.deltaTime;

        // FIRE
        if (timer >= timeBetweenAttack && _enemiesInRange.Any())
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        // Weapon delay reset
        timer = 0f;

        // Fire Projectile
        CalculateAimError();

        //spawn projectile
        var projectile =  Instantiate(towerProjectile.GetPrefab(), muzzlePosition.position, CalculateAimRotation(enemy.position));
        projectile.GetComponent<Rigidbody>().velocity =
            (enemy.position - projectile.transform.position).normalized * towerProjectile.Speed;
        // TODO audio.Play();
    }

    protected Quaternion CalculateAimRotation(Vector3 targetPosition)
    {
        // Aim at the targetPosition
        // TargetPosition substracting towerPosition creates a vector pointing from the tower to the targetPosition. 
        var aimPoint = new Vector3(targetPosition.x + aimError, 0, targetPosition.z + aimError) - transform.position;
        return Quaternion.LookRotation(aimPoint);
    }

    protected void CalculateAimError()
    {
        aimError = Random.Range(-errorAmount, errorAmount);
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