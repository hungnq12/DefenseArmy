using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] private List<StatData> statDatas;
    public List<StatConfig> StatConfigs(StatType type) => statDatas[(int)type].statConfigs;
}

[Serializable]
public class StatData
{
    public StatType statType;
    public List<StatConfig> statConfigs;
}

[Serializable]
public class StatConfig
{
    public float value;
    public int price;
}
