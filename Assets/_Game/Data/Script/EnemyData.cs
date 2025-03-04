using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private List<EnemyStat> enemyStats;
    public EnemyStat EnemyStat(int enemyID) => enemyStats[enemyID];
}

[Serializable]
public struct EnemyStat
{
    public EnemyController prefab;
    public float atk;
    public float hp;
    public float spd;
    public float atkSpd;
    public float range;
    public int reward;
}
