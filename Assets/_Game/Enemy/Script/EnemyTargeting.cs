using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    [SerializeField] private SphereCollider collider;
    [SerializeField] private float _atkRange;
    private EnemyController _enemyController;

    public void Init(EnemyController enemyController)
    {
        _enemyController = enemyController;
        collider.radius = _atkRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        var tower = other.GetComponent<TowerController>();
        if (tower != null)
        {
            _enemyController.InvokeStateChanged(EnemyState.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var tower = other.GetComponent<TowerController>();
        if (tower != null)
        {
            _enemyController.InvokeStateChanged(EnemyState.Move);
        }
    }
}

