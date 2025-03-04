using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform model;
    private TowerController _towerController;
    private IStatManager _statManager;
    private float _atkRange;
    private readonly List<EnemyController> _enemiesInRange = new List<EnemyController>();
    private EnemyController _currentTarget;

    public void Init(TowerController towerController, IStatManager statManager)
    {
        _towerController = towerController;
        _statManager = statManager;
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _statManager.OnStatChanged += UpdateAttackValue;
    }

    private void UnsubscribeEvents()
    {
        _statManager.OnStatChanged -= UpdateAttackValue;
    }

    private void UpdateAttackValue(StatType type, float value)
    {
        switch (type)
        {
            case StatType.AttackRange:
                _atkRange = value;
                break;
        }
    }

    private void Update()
    {
        RotateTowardsTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyController>();
        if (enemy == null) return;
        enemy.OnEnemyDie += RemoveTarget;
        _enemiesInRange.Add(enemy);
        FindNewTarget();

        void RemoveTarget(EnemyController enemy)
        {
            _enemiesInRange.Remove(enemy);
            enemy.OnEnemyDie -= RemoveTarget;
            _currentTarget = null;
            FindNewTarget();
        }
    }

    private void FindNewTarget()
    {
        if (_currentTarget == null)
        {
            _currentTarget = GetClosestTarget();
            _towerController.InvokeFoundTarget(_currentTarget);
        }
    }

    private EnemyController GetClosestTarget()
    {
        if (_enemiesInRange.Count <= 0) return null;
        EnemyController bestTarget = null;
        float minDistance = Mathf.Infinity;
        foreach (var target in _enemiesInRange)
        {
            if (target != null)
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestTarget = target;
                }
            }
        }

        return bestTarget;
    }

    private void RotateTowardsTarget()
    {
        if (_currentTarget == null) return;
        Vector3 direction = _currentTarget.transform.position - model.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            model.rotation = Quaternion.Slerp(model.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }
}