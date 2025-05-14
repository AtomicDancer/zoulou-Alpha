using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerMoney = 100;
    public TextMeshProUGUI moneyText;
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

        moneyText.text = playerMoney + " $";
    }

    public bool OnWaveStart()
    {
        return true; 
    }
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        moneyText.text = playerMoney + " $";
    }
    
    public void SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            moneyText.text = playerMoney + " $";
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
