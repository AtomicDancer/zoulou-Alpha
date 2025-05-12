using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LevelData currentLevel;
    public int starterMoney;
    public int currentMoney;

    private int currentWaveIndex = 0;
    private int activeEnemies = 0;
    private Dictionary<string, Transform> spawnerLookup;

    void Awake()
    {
        instance = this;
        BuildSpawnerLookup();
    }

    void Start()
    {
        currentMoney = starterMoney;
        StartCoroutine(RunLevel());
    }

    IEnumerator RunLevel()
    {
        for (int i = 0; i < currentLevel.waves.Count; i++)
        {
            currentWaveIndex = i;
            yield return StartCoroutine(RunWave(currentLevel.waves[i]));

            while (activeEnemies > 0)
                yield return null;

            GainResourcesPassive(50); // Per wave
        }

        Debug.Log("Level complete!");
    }

    IEnumerator RunWave(WaveData wave)
    {
        foreach (var spawn in wave.spawns)
        {
            foreach (var timedSpawn in spawn.timedSpawns)
            {
                StartCoroutine(SpawnEnemyAtTime(spawn.spawnerID, timedSpawn));
            }
            foreach (var loop in spawn.loopSpawns)
            {
                StartCoroutine(SpawnEnemyLoop(spawn.spawnerID, loop));
            }

        }
        yield return null;
    }

    IEnumerator SpawnEnemyAtTime(string spawnerID, TimedEnemySpawn timedSpawn)
    {
        yield return new WaitForSeconds(timedSpawn.delay);

        if (string.IsNullOrEmpty(spawnerID) || !spawnerLookup.TryGetValue(spawnerID, out var spawnerTransform))
        {
            Debug.LogWarning("Missing enemy prefab or spawner.");
            yield break; // Exit the coroutine early
        }

        Vector3 spawnPos = spawnerTransform.position;
        Instantiate(timedSpawn.enemyPrefab, spawnPos, Quaternion.identity);
        activeEnemies++;
    }
    IEnumerator SpawnEnemyLoop(string spawnerID, LoopSpawn loopSpawn)
    {
        yield return new WaitForSeconds(loopSpawn.startTime);

        if (string.IsNullOrEmpty(spawnerID) || !spawnerLookup.TryGetValue(spawnerID, out var spawnerTransform))
        {
            Debug.LogWarning("Missing enemy prefab or spawner.");
            yield break;
        }

        Vector3 spawnPos = spawnerTransform.position;

        for (int i = 0; i < loopSpawn.count; i++)
        {
            Instantiate(loopSpawn.enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies++;
            yield return new WaitForSeconds(loopSpawn.delay);
        }
    }

    public void OnEnemyKilled()
    {
        activeEnemies--;
    }

    void BuildSpawnerLookup()
    {
        spawnerLookup = new Dictionary<string, Transform>();
        foreach (var spawner in FindObjectsByType<Spawner>(FindObjectsSortMode.None))
        {
            if (!string.IsNullOrEmpty(spawner.spawnerID))
            {
                spawnerLookup[spawner.spawnerID] = spawner.transform;
            }
        }
    }

    #region Resources
    public void GainResourcesPassive(int amount) => currentMoney += amount;
    public void GainResourcesOnKill(int amount) => currentMoney += amount;
    public void LoseResources(int amount) => currentMoney -= amount;
    #endregion
}
