using System.Collections;
using UnityEngine;

public abstract class UnitsSkills : MonoBehaviour
{
    public float duration = 5f;
    public float costPerSecond = 1f;
    public float currentCost;
    public float cost;
    public bool skillReady = false;
    public bool isSkillActive = false;
    public BaseUnit unit;
    public abstract void UseSkill();

    void Start()
    {
        unit.skillChargeBar.fillAmount = 0;
        InvokeRepeating("GainCost", 0f, 1f);
    }

    public void GainCost()
    {
        if (currentCost < cost && !isSkillActive)
        {
            currentCost += costPerSecond;
            unit.skillChargeBar.fillAmount = currentCost / cost;
            if (currentCost >= cost)
            {
                skillReady = true;
                unit.skillReadyFxAura.Play();
            }
        }
    }
}
