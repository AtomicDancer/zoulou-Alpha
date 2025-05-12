using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaveEditorWindow : EditorWindow
{
    private LevelData levelData;
    private Vector2 scroll;
    private int selectedWaveIndex = 0;

    [MenuItem("Tools/Wave Editor")]
    public static void ShowWindow()
    {
        GetWindow<WaveEditorWindow>("Wave Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Wave Editor", EditorStyles.boldLabel);
        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);

        if (levelData == null)
            return;

        List<string> waveLabels = new List<string>();
        for (int i = 0; i < levelData.waves.Count; i++)
        {
            waveLabels.Add($"Wave {i + 1}");
        }
        waveLabels.Add("+ Add Wave");

        selectedWaveIndex = GUILayout.Toolbar(selectedWaveIndex, waveLabels.ToArray());

        // Handle Add Wave tab
        if (selectedWaveIndex == levelData.waves.Count)
        {
            levelData.waves.Add(new WaveData());
            selectedWaveIndex = levelData.waves.Count - 1;
            EditorUtility.SetDirty(levelData);
            return;
        }

        if (selectedWaveIndex < 0 || selectedWaveIndex >= levelData.waves.Count)
            return;

        var wave = levelData.waves[selectedWaveIndex];

        scroll = EditorGUILayout.BeginScrollView(scroll);

        GUILayout.Space(5);
        GUILayout.Label($"Wave {selectedWaveIndex + 1}", EditorStyles.boldLabel);
        for (int s = 0; s < wave.spawns.Count; s++)
        {
            var spawnEntry = wave.spawns[s];

            GUILayout.Label("Loop Spawns", EditorStyles.boldLabel);

            for (int l = 0; l < spawnEntry.loopSpawns.Count; l++)
            {
                var loop = spawnEntry.loopSpawns[l];
                EditorGUILayout.BeginHorizontal();
                loop.enemyPrefab = (GameObject)EditorGUILayout.ObjectField(loop.enemyPrefab, typeof(GameObject), false);
                loop.startTime = EditorGUILayout.FloatField("Start", loop.startTime);
                loop.delay = EditorGUILayout.FloatField("Interval", loop.delay);
                loop.count = EditorGUILayout.IntField("Count", loop.count);
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    spawnEntry.loopSpawns.RemoveAt(l);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add Loop Spawn"))
                spawnEntry.loopSpawns.Add(new LoopSpawn());

            EditorGUILayout.BeginVertical("box");
            spawnEntry.spawnerID = EditorGUILayout.TextField("Spawner ID", spawnEntry.spawnerID);

            for (int t = 0; t < spawnEntry.timedSpawns.Count; t++)
            {
                var timed = spawnEntry.timedSpawns[t];
                EditorGUILayout.BeginHorizontal();
                timed.enemyPrefab = (GameObject)EditorGUILayout.ObjectField(timed.enemyPrefab, typeof(GameObject), false);
                timed.delay = EditorGUILayout.FloatField("Delay", timed.delay);
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    spawnEntry.timedSpawns.RemoveAt(t);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Timed Spawn"))
                spawnEntry.timedSpawns.Add(new TimedEnemySpawn());

            if (GUILayout.Button("Remove Spawn Entry"))
            {
                wave.spawns.RemoveAt(s);
                break;
            }

            EditorGUILayout.EndVertical();
        }
        
        if (GUILayout.Button("Add Spawn Entry"))
            wave.spawns.Add(new WaveSpawnEntry());

        if (GUILayout.Button("Remove This Wave"))
        {
            levelData.waves.RemoveAt(selectedWaveIndex);
            selectedWaveIndex = Mathf.Clamp(selectedWaveIndex - 1, 0, levelData.waves.Count - 1);
        }

        EditorGUILayout.EndScrollView();

        if (GUI.changed)
            EditorUtility.SetDirty(levelData);
    }
}
