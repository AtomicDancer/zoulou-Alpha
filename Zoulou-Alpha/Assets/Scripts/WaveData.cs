using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/Wave")]
public class WaveData : ScriptableObject
{
    public List<WaveSpawnEntry> spawns = new List<WaveSpawnEntry>();
}