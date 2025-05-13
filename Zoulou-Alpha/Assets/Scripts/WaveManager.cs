using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public LaneSpawner spawner;
    public List<WaveDataNew> waves;
    public float timeBetweenWaves = 10f;

    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWave < waves.Count)
        {
            WaveDataNew wave = waves[currentWave];

            if (wave.isHugeWave)
                Debug.LogWarning("🌊 Huge wave incoming!");

            List<EnemySpawnInfo> spawns = wave.ExpandChunks();

            foreach (var spawn in spawns)
                StartCoroutine(SpawnEnemyWithDelay(spawn));

            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("✅ All waves done!");
    }

    private IEnumerator SpawnEnemyWithDelay(EnemySpawnInfo spawn)
    {
        yield return new WaitForSeconds(spawn.delay);
        spawner.SpawnEnemy(spawn.enemyPrefab, spawn.lane);
    }
}
