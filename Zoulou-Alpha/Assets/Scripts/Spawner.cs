using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spawner : MonoBehaviour
{
    public string spawnerID = "Spawner_";
    public List<TimedEnemySpawn> timedSpawns = new List<TimedEnemySpawn>();
    public List<LoopSpawn> loopSpawns = new List<LoopSpawn>();
}


#if UNITY_EDITOR
[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Spawner spawner = (Spawner)target;

        EditorGUILayout.LabelField("Spawner Unique ID", EditorStyles.boldLabel);
        spawner.spawnerID = EditorGUILayout.TextField("Spawner ID", spawner.spawnerID);

        if (GUILayout.Button("Auto-Generate ID"))
        {
            spawner.spawnerID = System.Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        EditorGUILayout.Space();
        
        // Display and edit the TimedEnemySpawns
        EditorGUILayout.LabelField("Timed Enemy Spawns", EditorStyles.boldLabel);
        for (int i = 0; i < spawner.timedSpawns.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            // Enemy prefab field
            spawner.timedSpawns[i].enemyPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy Prefab", spawner.timedSpawns[i].enemyPrefab, typeof(GameObject), false);

            // Delay field
            spawner.timedSpawns[i].delay = EditorGUILayout.FloatField("Delay (s)", spawner.timedSpawns[i].delay);

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                spawner.timedSpawns.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Timed Spawn"))
        {
            spawner.timedSpawns.Add(new TimedEnemySpawn());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(spawner);
        }
    }
}
#endif
