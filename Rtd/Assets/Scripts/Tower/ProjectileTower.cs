using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Mechanics;

/// <summary>
/// Tower with shooting ability
/// </summary>
public class ProjectileTower : TowerBase
{
    public Transform MuzzlePosition;
    public ProjectileBase TowerProjectile;
    protected Transform _enemy;    

    /// <summary>
    /// Shoot projectile on the closest harget
    /// </summary>
    public override void Attack()
    {        
        if (!_enemiesInRange.Any()) return;

        _enemy = GetClosestEnemy(_enemiesInRange);

        //spawn and fire projectile
        var projectile =  Instantiate(TowerProjectile.GetPrefab(), MuzzlePosition.position, CalculateAimRotation(_enemy.position));
        projectile.GetComponent<Rigidbody>().velocity = CalculateAimVector(_enemy);            
        // TODO audio.Play();
    }

    protected Quaternion CalculateAimRotation(Vector3 targetPosition)
    {
        return TargetingMechanis.CalculateAimRotation(targetPosition, transform.position);
    }

    /// <summary>
    /// Calculate vector that should be used for projectile velocity
    /// </summary>
    /// <param name="enemy">Enemy to shoot</param>
    /// <returns>Vector used for proejctile velocity</returns>
    protected Vector3 CalculateAimVector(Transform enemy)
    {
        return TargetingMechanis.CalculateAimVelocityVector(enemy, MuzzlePosition.position, TowerProjectile.Speed);
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

        for (var i = 0; i < enemies.Count; i++)
        {
            var potentialTarget = enemies[i];
            if (potentialTarget == null)
            {
                enemies.RemoveAt(i);
                continue;
            }
            

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