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
    public float StatValue(StatType type, int level) => StatConfigs(type)[level].value;
    
    /*#if UNITY_EDITOR
    [Button]
    private void UpdateConfig()
    {
        
    }
    #endif*/
}

[Serializable]
public struct StatData
{
    public StatType statType;
    public List<StatConfig> statConfigs;
}

[Serializable]
public struct StatConfig
{
    public float value;
    public int price;
}
