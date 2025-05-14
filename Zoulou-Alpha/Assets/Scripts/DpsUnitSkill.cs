using System.Collections;
using UnityEngine;

public class DpsUnitSkill : UnitsSkills
{
    public DpsUnit dpsUnit;

    public override void UseSkill()
    {
        if (!skillReady)
        {
            Debug.Log("Skill not ready yet!");
            return;
        }
        currentCost = 0;
        Debug.Log("DpsUnit skill used!");
        dpsUnit.damage *= 2; // Double the damage
        dpsUnit.attackCooldown /= 2; // Halve the attack cooldown
        DiminishSkillBarForDuration();
        StartCoroutine(ResetSkillEffect());
    }

    void DiminishSkillBarForDuration()
    {
        StartCoroutine(DiminishSkillBar());
    }

    IEnumerator DiminishSkillBar()
    {
        float elapsedTime = 0f;
        float initialFillAmount = unit.skillChargeBar.fillAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            unit.skillChargeBar.fillAmount = Mathf.Lerp(initialFillAmount, 0, elapsedTime / duration);
            yield return null;
        }

        unit.skillChargeBar.fillAmount = 0;
    }

    IEnumerator ResetSkillEffect()
    {
        skillReady = false;
        unit.skillReadyFxAura.Stop();
        isSkillActive = true;
        yield return new WaitForSeconds(duration); 
        dpsUnit.damage /= 2; // Reset damage
        dpsUnit.attackCooldown *= 2; // Reset attack cooldown
        Debug.Log("Skill effect ended.");
        isSkillActive = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            UseSkill();
        }
    }
}
