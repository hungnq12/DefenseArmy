using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    [SerializeField] private Transform model;
    private TowerController _towerController;
    private IStatManager _statManager;
    private readonly List<EnemyController> _enemiesInRange = new List<EnemyController>();
    private EnemyController _currentTarget;
    private Tween _rotateModel;

    public void Init(TowerController towerController, IStatManager statManager)
    {
        _towerController = towerController;
        _statManager = statManager;
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _statManager.OnStatChanged += UpdateAttackValue;
        _towerController.OnTowerDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents()
    {
        _currentTarget = null;
        DOTween.Kill(_rotateModel);
        _statManager.OnStatChanged -= UpdateAttackValue;
        _towerController.OnTowerDie -= UnsubscribeEvents;
    }

    public void StartLevel()
    {
        var atkRange = _statManager.StatValue(StatType.AttackRange, _statManager.StatLevelIngame[StatType.AttackRange]);
        transform.localScale = Vector3.one * atkRange / 50f;
    }

    private void UpdateAttackValue(StatType type, float value)
    {
        switch (type)
        {
            case StatType.AttackRange:
                transform.DOScale(value / 50f, 0.1f);
                break;
        }
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
            RotateTowardsTarget();
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
            _rotateModel = model.DORotateQuaternion(desiredRotation, 0.5f)
                .OnComplete(() => _towerController.InvokeFoundTarget(_currentTarget))
                .OnUpdate(() =>
                {
                    if (_currentTarget == null || !_currentTarget.IsAlive)
                        DOTween.Kill(_rotateModel);
                });
        }
    }
}