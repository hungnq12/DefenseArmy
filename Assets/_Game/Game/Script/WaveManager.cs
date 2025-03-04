using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private float radius;
    [SerializeField] private float spawnTime = 1f;
    private float _spawnTimer;
    private ICurrencyManager _currencyManager;
    private Transform _target;

    public void Init(ICurrencyManager currencyManager, Transform target)
    {
        _currencyManager = currencyManager;
        _target = target;
    }

    private void Update()
    {
        StartWave();
    }

    public void StartWave()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > spawnTime)
        {
            _spawnTimer = 0;
            var enemy = PoolManager.Instance.GetObject(enemyPrefab);
            enemy.Init(GetRandomSpawnPosition(_target.position, radius), _target);
            enemy.OnEnemyDie -= HandleEnemyDie;
            enemy.OnEnemyDie += HandleEnemyDie;
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 centerPos, float radius)
    {
        float angle = Random.Range(0f, 2 * Mathf.PI);
        float x = centerPos.x + radius * Mathf.Cos(angle);
        float z = centerPos.z + radius * Mathf.Sin(angle);
        return new Vector3(x, 0f, z);
    }

    private void HandleEnemyDie(EnemyController enemyController)
    {
        _currencyManager.AddCurrency(CurrencyType.Silver, 1);
    }
}