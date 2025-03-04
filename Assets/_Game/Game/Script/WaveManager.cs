using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public interface IWaveManager
{
    int CurrentLevelID { get; }
    int CurrentWaveID { get; }
    event Action<int, int> OnEnemyCountChanged;
    event Action<int> OnWaveChanged;
    event Action OnKilledAllWave;
    void StartSpawn();
    void KillAll();
}
public class WaveManager : MonoBehaviour, IWaveManager
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private SilverDropVFX silverDropVFXPrefab;
    [SerializeField] private float radius;
    private ICurrencyManager _currencyManager;
    private Transform _target;
    private int _enemiesAliveInWave;
    private int EnemiesAliveInWave
    {
        get => _enemiesAliveInWave;
        set
        {
            _enemiesAliveInWave = value;
            OnEnemyCountChanged?.Invoke(_enemiesInWave - _enemiesAliveInWave, _enemiesInWave);
        }
    }
    private int _enemiesInWave;
    private Coroutine _spawnCoroutine;
    private event Action OnKillAllEnemy;

    public int CurrentLevelID { get; private set; }
    public int CurrentWaveID { get; private set; }
    public event Action<int, int> OnEnemyCountChanged;
    public event Action<int> OnWaveChanged;
    public event Action OnKilledAllWave;

    public void Init(ICurrencyManager currencyManager, Transform target)
    {
        _currencyManager = currencyManager;
        _target = target;
    }

    public void StartSpawn()
    {
        CurrentLevelID = PlayerPrefs.GetInt("CurrentLevelID", 0);
        var level = levelData.Level(CurrentLevelID);
        _spawnCoroutine = StartCoroutine(Spawn(level));
    }

    public void KillAll()
    {
        OnKillAllEnemy?.Invoke();
    }
    
    private IEnumerator Spawn(Level level)
    {
        foreach (var w in level.waves)
        {
            yield return StartCoroutine(SpawnWave(w));
            yield return new WaitUntil(() => EnemiesAliveInWave <= 0);
        }
        OnKilledAllWave?.Invoke();
    }
    private IEnumerator SpawnWave(Wave wave)
    {
        CurrentWaveID = wave.waveID;
        OnWaveChanged?.Invoke(CurrentWaveID);
        EnemiesAliveInWave = _enemiesInWave = wave.TotalEnemyInWave();
        foreach (var mw in wave.miniWaves)
        {
            yield return new WaitForSeconds(mw.spawnDelay);

            for (int i = 0; i < mw.count; i++)
            {
                SpawnEnemy(mw.enemyID);
            }
        }
    }

    private void SpawnEnemy(int enemyID)
    {
        var enemyStat = enemyData.EnemyStat(enemyID);
        var enemy = PoolManager.Instance.GetObject(enemyStat.prefab);
        enemy.Init(GetRandomSpawnPosition(_target.position, radius), _target, enemyStat);
        enemy.OnEnemyDie += HandleEnemyDie;
        OnKillAllEnemy += enemy.ReturnToPool;
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
        _currencyManager.AddCurrency(CurrencyType.Silver, enemyController.EnemyStat.reward);
        var vfx = PoolManager.Instance.GetObject(silverDropVFXPrefab, isUI: true);
        vfx.Show(enemyController.EnemyStat.reward, Camera.main.WorldToScreenPoint(enemyController.transform.position));
        EnemiesAliveInWave--;
        
        OnKillAllEnemy -= enemyController.ReturnToPool;
    }
}