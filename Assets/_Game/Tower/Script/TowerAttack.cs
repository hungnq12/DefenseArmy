using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private ParticlePoolable gunMuzzlePrefab;
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
    
    private void SubscribeEvents()
    {
        _statManager.OnStatChanged += UpdateAttackValue;
        _towerController.OnFoundTarget += UpdateTarget;
        _towerController.OnTowerDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents()
    {
        _currentTarget = null;
        _statManager.OnStatChanged -= UpdateAttackValue;
        _towerController.OnFoundTarget -= UpdateTarget;
        _towerController.OnTowerDie -= UnsubscribeEvents;
    }

    public void StartLevel()
    {
        _atk = _statManager.StatValue(StatType.Attack, _statManager.StatLevelIngame[StatType.Attack]);
        _atkSpd = _statManager.StatValue(StatType.AttackSpeed, _statManager.StatLevelIngame[StatType.AttackSpeed]);
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
        _shootTimer += Time.deltaTime * _atkSpd;
        if (_currentTarget != null && _currentTarget.IsAlive && _shootTimer >= 1f)
        {
            _shootTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        _towerController.InvokeStateChanged(TowerState.Attack);
        var projectile = PoolManager.Instance.GetObject(projectilePrefab);
        projectile.Shoot(_atk, shootPos.position, shootPos.forward.normalized);

        var muzzle = PoolManager.Instance.GetObject(gunMuzzlePrefab);
        muzzle.Show(shootPos.position, shootPos.forward.normalized, Vector3.one * 0.25f);
    }
}
