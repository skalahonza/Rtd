using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Mechanics;
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
        projectile.GetComponent<Rigidbody>().velocity = CalculateAimVector(_enemy);            
        // TODO audio.Play();
    }

    protected Quaternion CalculateAimRotation(Vector3 targetPosition)
    {
        // Aim at the targetPosition
        // TargetPosition substracting towerPosition creates a vector pointing from the tower to the targetPosition. 
        var aimPoint = new Vector3(targetPosition.x, 0, targetPosition.z) - transform.position;
        return Quaternion.LookRotation(aimPoint);
    }

    protected Vector3 CalculateAimVector(Transform enemy)
    {
        return TargetingMechanis.CalculateAimVector(enemy, MuzzlePosition.position, TowerProjectile.Speed);
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