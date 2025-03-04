using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private DamageTextVFX damageTextVFXPrefab;
    private EnemyController _enemyController;
    private float _currentHP;
    public bool IsAlive => _currentHP > 0;

    public void Init(EnemyController enemyController, float hp)
    {
        _enemyController = enemyController;
        _currentHP = hp;
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
            _enemyController.InvokeStateChanged(EnemyState.Die);
            _enemyController.InvokeEnemyDie();
        }
        
        var vfx = PoolManager.Instance.GetObject(damageTextVFXPrefab, isUI: true);
        vfx.Show(damage, Camera.main.WorldToScreenPoint(transform.position));
    }
}
