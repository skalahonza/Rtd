using System.Collections.Generic;
using Assets.Scripts.Constants;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public abstract class TowerBase : MonoBehaviour
{
    public float TimeBetweenAttack;
    public float Radius;

    protected float _timer;

    protected readonly List<Transform> _enemiesInRange = new List<Transform>();    

    /// <summary>
    /// Execute attack on one or more enemiesInRange
    /// </summary>
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