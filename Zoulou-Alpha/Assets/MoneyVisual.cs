using UnityEngine;
using TMPro;

public class MoneyVisual : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = SaveManager.Instance.CurrentSave.playerMoney.ToString() + " $";
    }
}
