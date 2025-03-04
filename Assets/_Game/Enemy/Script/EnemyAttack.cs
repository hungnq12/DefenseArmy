using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private AnimationEvent animationEvent;
    private EnemyController _enemyController;
    private float _atk;
    private float _atkSpd;
    private float _shootTimer;
    private TowerController _currentTarget;

    public void Init(EnemyController enemyController, float atk, float atkSpd)
    {
        _enemyController = enemyController;
        _atk = atk;
        _atkSpd = atkSpd;
        _currentTarget = null;
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _enemyController.OnFoundTarget += UpdateTarget;
        _enemyController.OnEnemyDie += UnsubscribeEvents;
        animationEvent.OnAttackFrame += AttackFrame;
    }

    private void UnsubscribeEvents(EnemyController _)
    {
        _enemyController.OnFoundTarget -= UpdateTarget;
        _enemyController.OnEnemyDie -= UnsubscribeEvents;
        animationEvent.OnAttackFrame -= AttackFrame;
    }

    private void UpdateTarget(TowerController target)
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
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        _enemyController.InvokeStateChanged(EnemyState.Attack);
    }

    private void AttackFrame()
    {
        _currentTarget.InvokeTowerTakeDamage(_atk);
    }
}
