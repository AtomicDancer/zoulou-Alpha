using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Unit Data")]
public class UnitData : ScriptableObject
{
    public string unitID; // Unique ID (e.g., "Peashooter01")
    public Sprite icon;
    public int costToBuy;
}
