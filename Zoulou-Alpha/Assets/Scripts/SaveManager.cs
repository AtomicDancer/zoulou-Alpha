using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;
    public SaveData CurrentSave;
    public MoneyVisual moneyVisual;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save.json";
            LoadGame();
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CurrentSave = new SaveData();
            SaveGame();
            Debug.Log("Game saved!");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddMoney(10000);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            CurrentSave.playerMoney -= 10000;
            if(moneyVisual != null)
                moneyVisual.UpdateMoneyText();
            SaveGame();
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(CurrentSave, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CurrentSave = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            CurrentSave = new SaveData();
        }
    }

    public bool IsUnitUnlocked(string unitID) =>
        CurrentSave.unlockedUnitIDs.Contains(unitID);

    public void UnlockUnit(string unitID)
    {
        if (!IsUnitUnlocked(unitID))
        {
            CurrentSave.unlockedUnitIDs.Add(unitID);
            SaveGame();
        }
    }

    public void AddMoney(int amount)
    {
        CurrentSave.playerMoney += amount;
        if(moneyVisual != null)
            moneyVisual.UpdateMoneyText();
        SaveGame();
    }

    public bool SpendMoney(int amount)
    {
        if (CurrentSave.playerMoney >= amount)
        {
            CurrentSave.playerMoney -= amount;
            if(moneyVisual != null)
                moneyVisual.UpdateMoneyText();
            SaveGame();
            return true;
        }
        return false;
    }
}
