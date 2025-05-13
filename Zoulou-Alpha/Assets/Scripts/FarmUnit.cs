using UnityEngine;

public class FarmUnit : BaseUnit
{
    public int incomePerInterval = 10;
    public float interval = 5f;
    public float range = 3f;
    private float timer;
    private bool statsVisible = false;

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

    public bool AreStatsVisible()
    {
        return statsVisible;
    }

    public void ShowStats()
    {
        statsVisible = true;
        // Add logic to display stats
    }

    public void HideStats()
    {
        statsVisible = false;
        // Add logic to hide stats
    }
}

