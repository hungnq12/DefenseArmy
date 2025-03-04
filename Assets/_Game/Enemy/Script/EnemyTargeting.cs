using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    [SerializeField] private SphereCollider collider;
    private EnemyController _enemyController;

    public void Init(EnemyController enemyController, float range)
    {
        _enemyController = enemyController;
        collider.radius = range * 0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        var tower = other.GetComponent<TowerController>();
        if (tower != null)
        {
            _enemyController.InvokeStateChanged(EnemyState.Attack);
            _enemyController.InvokeFoundTarget(tower);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var tower = other.GetComponent<TowerController>();
        if (tower != null)
        {
            _enemyController.InvokeStateChanged(EnemyState.Move);
            _enemyController.InvokeFoundTarget(null);
        }
    }
}

