using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatManager
{
    Dictionary<StatType, int> StatLevel { get; }
    Dictionary<StatType, int> StatLevelIngame { get; }
    TowerData TowerData { get; }
    event Action<StatType, float> OnStatChanged;
    void UpgradeStat(StatType type);
}
public class StatManager : MonoBehaviour, IStatManager
{
    [SerializeField] private TowerData towerData;
    public TowerData TowerData => towerData;
    public Dictionary<StatType, int> StatLevel { get; } = new();
    public Dictionary<StatType, int> StatLevelIngame { get; } = new();

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
        var newStatValue = TowerData.StatConfigs(type)[StatLevelIngame[type]].value;
        OnStatChanged?.Invoke(type, newStatValue);
    }
}

public enum StatType
{
    Attack, AttackSpeed, AttackRange
}