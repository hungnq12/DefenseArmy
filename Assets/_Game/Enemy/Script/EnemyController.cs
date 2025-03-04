using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Move, Attack, Die
}
public class EnemyController : IPoolable
{
    [SerializeField] private EnemyMove enemyMove;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyTargeting enemyTargeting;
    [SerializeField] private EnemyHP enemyHp;
    [SerializeField] private EnemyAnimation enemyAnimation;

    public bool IsAlive => enemyHp.IsAlive;
    
    public event Action<float> OnEnemyTakeDamage;
    public event Action<EnemyController> OnEnemyDie;
    public event Action<EnemyState> OnStateChanged;
    
    public void Init(Vector3 currentPos, Transform target)
    {
        transform.position = currentPos;
        enemyMove.Init(this, target);
        enemyAttack.Init(this);
        enemyTargeting.Init(this);
        enemyHp.Init(this);
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
        PoolManager.Instance.ReturnObject(this);
    }

    public void InvokeStateChanged(EnemyState state)
    {
        OnStateChanged?.Invoke(state);
    }
}
