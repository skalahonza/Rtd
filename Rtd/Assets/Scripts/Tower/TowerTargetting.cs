using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Constants;

public class TowerTargetting : MonoBehaviour
{

    public float timeBetweenAttack = 0.5f;
    public int attackDamage = 10;
    public float turretTurnSpeed = 3.5f;
    public float firePauseTime = 0.25f;
    public float errorAmount = 2f;
    public float reloadTime = 2f;

    public Transform turretPivot;
    public Transform muzzlePosition;
    public GameObject towerProjectile;

    float aimError;
    float timer;

    private List<Transform> EnemiesInRange = new List<Transform>();
    public Transform enemy;
    private Quaternion desiredRotation;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameTag.Player.ToString())
        {
            // Add object which enters collider to List
            EnemiesInRange.Add(other.transform);

            // If there is only one item in the List set it as target
            if (EnemiesInRange.Any())
            {
                enemy = GetClosestEnemy(EnemiesInRange);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GameTag.Player.ToString())
        {

            // Remove object which leaves the collider from List
            EnemiesInRange.Remove(other.transform);

            // If the List has objects in it set the closest as target
            if (EnemiesInRange.Any())
            {
                // TO-DO Turret turn delay
                enemy = GetClosestEnemy(EnemiesInRange);
            }
            else
            {
                enemy = null;
                EnemiesInRange.Clear();
            }
        }
    }


    void Update()
    {
        timer += Time.deltaTime;

        // Aim
        if (EnemiesInRange.Count != 0)
        {
            // Where to aim
            CalculateAimPosition(enemy.position);
            // Rotate the turrent towards the target
            turretPivot.rotation = Quaternion.Lerp(turretPivot.rotation, desiredRotation, Time.deltaTime * turretTurnSpeed);
        }

        // FIRE

        if (timer >= timeBetweenAttack && EnemiesInRange.Count != 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Weapon delay reset
        timer = 0f;

        // Fire Projectile
        CalculateAimError();

        Instantiate(towerProjectile, muzzlePosition.position, muzzlePosition.rotation);
        // TODO audio.Play();
    }

    private void CalculateAimPosition(Vector3 targetPosition)
    {
        // Aim at the targetPosition
        // TargetPosition substracting towerPosition creates a vector pointing from the tower to the targetPosition. 
        var aimPoint = new Vector3(targetPosition.x + aimError, 0, targetPosition.z + aimError) - transform.position;
        desiredRotation = Quaternion.LookRotation(aimPoint);
    }

    private void CalculateAimError()
    {
        aimError = Random.Range(-errorAmount, errorAmount);
    }

    public Transform GetClosestEnemy(List<Transform> enemies)
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