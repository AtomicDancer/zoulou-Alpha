using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerMoney = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool OnWaveStart()
    {
        return true; 
    }
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log("Money: " + playerMoney);
    }
    
    public void SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            Debug.Log("Spent: " + amount + ", Remaining: " + playerMoney);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
