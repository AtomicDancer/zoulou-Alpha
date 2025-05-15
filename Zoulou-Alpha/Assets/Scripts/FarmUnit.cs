using UnityEngine;

public class FarmUnit : BaseUnit
{
    public int incomePerInterval = 10;
    public float interval = 5f;
    public float range = 3f;
    private float timer;
    private bool statsVisible = false;

    void Start()
    {
        rangeVisualPrefab.transform.localScale = new Vector3(range / 2.5f, 0.01f, range / 2.5f);
        rangeVisualPrefab.SetActive(false);
        timer = interval; // Start the timer at the interval to avoid immediate income generation
    }

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
        rangeVisualPrefab.SetActive(true);
        // Add logic to display stats
    }

    public void HideStats()
    {
        statsVisible = false;
        rangeVisualPrefab.SetActive(false);
        // Add logic to hide stats
    }
}

