using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatManager
{
    TowerData TowerData { get; }
    Dictionary<StatType, int> StatLevel { get; }
    Dictionary<StatType, int> StatLevelIngame { get; }
    float StatValue(StatType type, int level);
    event Action<StatType, float> OnStatChanged;
    void UpgradeStat(StatType type);
}
public class StatManager : MonoBehaviour, IStatManager
{
    [SerializeField] private TowerData towerData;
    public TowerData TowerData => towerData;
    public Dictionary<StatType, int> StatLevel { get; } = new();
    public Dictionary<StatType, int> StatLevelIngame { get; } = new();
    public float StatValue(StatType type, int level) => towerData.StatValue(type, level);

    public event Action<StatType, float> OnStatChanged;

    public void Init()
    {
        for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
        {
            StatLevel[(StatType)i] = StatLevelIngame[(StatType)i] = PlayerPrefs.GetInt($"{(StatType)i}Level", 0);
        }
    }

    public void UpgradeStat(StatType type)
    {
        StatLevelIngame[type]++;
        var newStatValue = StatValue(type, StatLevelIngame[type]);
        OnStatChanged?.Invoke(type, newStatValue);
    }
}

public enum StatType
{
    Attack, AttackSpeed, AttackRange
}