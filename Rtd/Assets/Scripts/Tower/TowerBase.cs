using System.Collections.Generic;
using Assets.Scripts.Constants;
using UnityEngine;

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
    }

    public virtual void Update()
    {
        _timer += Time.deltaTime;

        //update targets
        _enemiesInRange.Clear();
        var colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (var other in colliders)
        {
            if (other.gameObject.tag == GameTag.Player.ToString())
            {
                // Add object which enters collider to List
                _enemiesInRange.Add(other.transform);
            }
        }

        // FIRE
        if (_timer >= TimeBetweenAttack)
        {
            // Weapon delay reset
            _timer = 0f;
            Attack();
        }
    }
}