using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public LaneSpawner spawner;
    public List<LoopingWaveBlock> loopingWaves;
    public float timeBetweenWaves = 10f;

    private void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        foreach (var block in loopingWaves)
        {
            int loops = block.loopCount < 0 ? int.MaxValue : block.loopCount;

            for (int i = 0; i < loops; i++)
            {
                foreach (var wave in block.waveSequence)
                {
                    if (wave.isHugeWave)
                        Debug.LogWarning("🌊 Huge wave incoming!");

                    List<EnemySpawnInfo> spawns = wave.ExpandChunks();

                    foreach (var spawn in spawns)
                        StartCoroutine(SpawnEnemyWithDelay(spawn));

                    yield return new WaitForSeconds(timeBetweenWaves);
                }
            }
        }

        Debug.Log("✅ All wave loops complete!");
    }

    private IEnumerator SpawnEnemyWithDelay(EnemySpawnInfo spawn)
    {
        yield return new WaitForSeconds(spawn.delay);
        spawner.SpawnEnemy(spawn.enemyPrefab, spawn.lane);
    }

}

[System.Serializable]
public class LoopingWaveBlock
{
    public List<WaveDataNew> waveSequence;
    public int loopCount = 1; // -1 = infinite
    public float loopDifficultyMultiplier = 1.0f; // Not used yet, placeholder for scaling
}