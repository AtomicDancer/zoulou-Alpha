using UnityEngine;

public class LaneSpawner : MonoBehaviour
{
    public Transform[] laneSpawnPoints; // Assign lane positions in Inspector

    public void SpawnEnemy(GameObject prefab, int lane)
    {
        if (lane < 0 || lane >= laneSpawnPoints.Length) return;
        Instantiate(prefab, laneSpawnPoints[lane].position, Quaternion.identity);
    }
}
