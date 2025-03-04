using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Move, Attack, Die
}
public class EnemyController : Poolable
{
    [SerializeField] private EnemyMove enemyMove;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyTargeting enemyTargeting;
    [SerializeField] private EnemyHP enemyHp;
    [SerializeField] private EnemyAnimation enemyAnimation;
    private EnemyStat _enemyStat;

    public bool IsAlive => enemyHp.IsAlive;
    public EnemyStat EnemyStat => _enemyStat;
    
    public event Action<float> OnEnemyTakeDamage;
    public event Action<EnemyController> OnEnemyDie;
    public event Action<TowerController> OnFoundTarget;
    public event Action<EnemyState> OnStateChanged;
    
    public void Init(Vector3 currentPos, Transform target, EnemyStat enemyStat)
    {
        transform.position = currentPos;
        _enemyStat = enemyStat;
        enemyMove.Init(this, target, _enemyStat.spd);
        enemyAttack.Init(this, _enemyStat.atk, _enemyStat.atkSpd);
        enemyTargeting.Init(this, _enemyStat.range);
        enemyHp.Init(this, _enemyStat.hp);
        enemyAnimation.Init(this);
        
        InvokeStateChanged(EnemyState.Move);
    }

    public void InvokeEnemyTakeDamage(float damage)
    {
        OnEnemyTakeDamage?.Invoke(damage);
    }

    public void InvokeEnemyDie()
    {
        OnEnemyDie?.Invoke(this);
        
        Invoke(nameof(ReturnToPool), 2f);
        OnEnemyDie = null;
    }

    public void InvokeStateChanged(EnemyState state)
    {
        OnStateChanged?.Invoke(state);
    }

    public void InvokeFoundTarget(TowerController target)
    {
        OnFoundTarget?.Invoke(target);
    }
    
    public void ReturnToPool() => PoolManager.Instance.ReturnObject(this);
}
