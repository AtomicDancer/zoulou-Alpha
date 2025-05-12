using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/Level")]
public class LevelData : ScriptableObject
{
    public List<WaveData> waves = new List<WaveData>();
}