using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform shootPos;
    private TowerController _towerController;
    private IStatManager _statManager;
    [SerializeField] private float _atk;
    [SerializeField] private float _atkSpd;
    private EnemyController _currentTarget;
    private float _shootTimer;
    
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
        _towerController.OnFoundTarget += UpdateTarget;
    }

    private void UnsubscribeEvents()
    {
        _statManager.OnStatChanged -= UpdateAttackValue;
        _towerController.OnFoundTarget -= UpdateTarget;
    }

    private void UpdateAttackValue(StatType type, float value)
    {
        switch (type)
        {
            case StatType.Attack:
                _atk = value;
                break;
            case StatType.AttackSpeed:
                _atkSpd = value;
                break;
        }
    }

    private void UpdateTarget(EnemyController target)
    {
        _currentTarget = target;
    }
    
    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if (_currentTarget == null) return;
        _shootTimer += Time.deltaTime * _atkSpd;
        if (_shootTimer >= 1f)
        {
            _shootTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!_currentTarget.IsAlive) return;
        _towerController.InvokeTowerAttack(_atk);
        var projectile = PoolManager.Instance.GetObject(projectilePrefab);
        projectile.Shoot(_atk, shootPos.position, shootPos.forward.normalized);
    }
}
