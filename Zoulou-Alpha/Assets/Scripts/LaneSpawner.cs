using UnityEngine;

public class LaneSpawner : MonoBehaviour
{
    public Transform[] laneSpawnPoints;       
    public WaypointPath[] laneWaypointPaths;    

    public void SpawnEnemy(GameObject prefab, int lane)
    {
        if (lane < 0 || lane >= laneSpawnPoints.Length) return;

        GameObject enemy = Instantiate(prefab, laneSpawnPoints[lane].position, Quaternion.identity);
        
        EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();
        behaviour.path = laneWaypointPaths[lane];
    }
}
