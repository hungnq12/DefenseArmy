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
    [SerializeField] private TowerHP towerHP;
    [SerializeField] private TowerAnimation towerAnimation;
    
    public bool IsAlive => towerHP.IsAlive;
    public event Action<TowerState> OnStateChanged;
    public event Action<EnemyController> OnFoundTarget;
    public event Action<float> OnTowerTakeDamage;
    public event Action OnTowerDie;
    public event Action<float, float> OnHPChanged;

    public void Init(IStatManager statManager)
    {
        towerAttack.Init(this, statManager);
        towerTargeting.Init(this, statManager);
        towerHP.Init(this, statManager);
        towerAnimation.Init(this);
    }

    public void StartLevel()
    {
        towerAttack.StartLevel();
        towerTargeting.StartLevel();
        towerHP.StartLevel();
    }

    public void InvokeStateChanged(TowerState state)
    {
        OnStateChanged?.Invoke(state);
    }

    public void InvokeFoundTarget(EnemyController target)
    {
        OnFoundTarget?.Invoke(target);
    }
    
    public void InvokeTowerTakeDamage(float damage)
    {
        OnTowerTakeDamage?.Invoke(damage);
    }

    public void InvokeTowerDie()
    {
        OnTowerDie?.Invoke();
    }

    public void InvokeHPChanged(float curHP, float maxHP)
    {
        OnHPChanged?.Invoke(curHP, maxHP);
    }
}
