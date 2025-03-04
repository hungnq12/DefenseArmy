using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    private EnemyController _enemyController;
    private float _currentHP;
    [SerializeField] private float _maxHP;
    public bool IsAlive => _currentHP > 0;

    public void Init(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _currentHP = _maxHP;
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _enemyController.OnEnemyTakeDamage += TakeDamage;
        _enemyController.OnEnemyDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents(EnemyController _)
    {
        _enemyController.OnEnemyTakeDamage -= TakeDamage;
        _enemyController.OnEnemyDie -= UnsubscribeEvents;
    }

    private void TakeDamage(float damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _enemyController.InvokeEnemyDie();
        }
    }
}
