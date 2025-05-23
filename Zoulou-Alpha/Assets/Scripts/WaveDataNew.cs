using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Wave", menuName = "TD/Wave Data")]
public class WaveDataNew : ScriptableObject
{
    public List<EnemyWaveChunk> waveChunks;
    public bool isHugeWave;

    public List<EnemySpawnInfo> ExpandChunks()
    {
        var spawnList = new List<EnemySpawnInfo>();
        foreach (var chunk in waveChunks)
        {
            for (int r = 0; r < Mathf.Max(1, chunk.repeat); r++)
            {
                for (int i = 0; i < chunk.count; i++)
                {
                    spawnList.Add(new EnemySpawnInfo
                    {
                        enemyPrefab = chunk.enemyPrefab,
                        lane = chunk.lane,
                        delay = chunk.startDelay + r * chunk.delayBetweenRepeats + i * chunk.interval
                    });
                }
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
    public int repeat = 1;
    public float delayBetweenRepeats = 0f;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int lane;
    public float delay;
}