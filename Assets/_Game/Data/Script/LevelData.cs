using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private List<Level> levels;
    public Level Level(int levelID) => levels[levelID];
}

[Serializable]
public struct Level
{
    public List<Wave> waves;
}
[Serializable]
public struct Wave
{
    public int waveID;
    public List<MiniWave> miniWaves;
    public int TotalEnemyInWave()
    {
        int count = 0;
        foreach (MiniWave miniWave in miniWaves)
            count += miniWave.count;
        return count;
    }
}
[Serializable]
public struct MiniWave
{
    public int enemyID;
    public int count;
    public float spawnDelay;
}