using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> unlockedUnitIDs = new(); // IDs of units the player owns
    public int playerMoney;
}

