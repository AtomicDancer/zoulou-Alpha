using UnityEngine;

public class FarmUnit : BaseUnit
{
    public int incomePerInterval = 10;
    public float interval = 5f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            GenerateIncome();
            timer = 0f;
        }
    }

    void GenerateIncome()
    {
        GameManager.Instance.AddMoney(incomePerInterval);
        // Optional: animation, SFX, particle effect
    }
}

