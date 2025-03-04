using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerState
{
    Idle, Move, Attack, Die
}
public class TowerController : MonoBehaviour
{
    [SerializeField] private TowerAttack towerAttack;
    [SerializeField] private TowerTargeting towerTargeting;
    [SerializeField] private TowerAnimation towerAnimation;
    
    public event Action<float> OnTowerAttack;
    public event Action<EnemyController> OnFoundTarget;

    public void Init(IStatManager statManager)
    {
        towerAttack.Init(this, statManager);
        towerTargeting.Init(this, statManager);
        towerAnimation.Init(this);
    }

    public void InvokeTowerAttack(float damage)
    {
        OnTowerAttack?.Invoke(damage);
    }

    public void InvokeFoundTarget(EnemyController target)
    {
        OnFoundTarget?.Invoke(target);
    }
}
