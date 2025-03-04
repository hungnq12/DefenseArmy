using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHP : MonoBehaviour
{
    private TowerController _towerController;
    private IStatManager _statManager;
    [SerializeField] private float _currentHP;
    [SerializeField] private float _maxHP = 120f;
    public bool IsAlive => _currentHP > 0;
    
    public void Init(TowerController towerController, IStatManager statManager)
    {
        _towerController = towerController;
        _statManager = statManager;
        SubscribeEvents();
    }
    
    private void SubscribeEvents()
    {
        _towerController.OnTowerTakeDamage += TakeDamage;
        _towerController.OnTowerDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents()
    {
        _towerController.OnTowerTakeDamage -= TakeDamage;
        _towerController.OnTowerDie -= UnsubscribeEvents;
    }

    public void StartLevel()
    {
        _currentHP = _maxHP;
        _towerController.InvokeHPChanged(_currentHP, _maxHP);
    }

    private void TakeDamage(float damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            _towerController.InvokeStateChanged(TowerState.Die);
            _towerController.InvokeTowerDie();
        }
        _towerController.InvokeHPChanged(_currentHP, _maxHP);
    }
}
