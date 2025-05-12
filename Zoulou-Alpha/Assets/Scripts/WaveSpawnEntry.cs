using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedEnemySpawn
{
    public GameObject enemyPrefab;
    public float delay;
}

[System.Serializable]
public class WaveSpawnEntry
{
    public string spawnerID;
    public List<TimedEnemySpawn> timedSpawns = new List<TimedEnemySpawn>();
    public List<LoopSpawn> loopSpawns = new List<LoopSpawn>();
}
