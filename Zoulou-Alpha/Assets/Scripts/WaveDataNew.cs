using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Wave", menuName = "TD/Wave Data")]
public class WaveDataNew : ScriptableObject
{
    public List<EnemyWaveChunk> waveChunks;
    public bool isHugeWave;

    // Expands chunks into individual spawn instructions
    public List<EnemySpawnInfo> ExpandChunks()
    {
        var spawnList = new List<EnemySpawnInfo>();
        foreach (var chunk in waveChunks)
        {
            for (int i = 0; i < chunk.count; i++)
            {
                spawnList.Add(new EnemySpawnInfo
                {
                    enemyPrefab = chunk.enemyPrefab,
                    lane = chunk.lane,
                    delay = chunk.startDelay + i * chunk.interval
                });
            }
        }
        return spawnList;
    }
}

[System.Serializable]
public class EnemyWaveChunk
{
    public GameObject enemyPrefab;
    public int lane;
    public int count = 1;
    public float interval = 1f;
    public float startDelay = 0f;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int lane;
    public float delay;
}
