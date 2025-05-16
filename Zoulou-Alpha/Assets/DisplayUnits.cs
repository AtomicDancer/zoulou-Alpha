using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUnits : MonoBehaviour
{
    public GameObject prefabToShow;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI costText;

    [Header("Stats Text")]
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI costToDeployText;
    private bool isShowing = false;
    private DpsUnit dpsUnit;
    private FarmUnit farmUnit;
    void Start()
    {
        prefabToShow.TryGetComponent<DpsUnit>(out dpsUnit);
        prefabToShow.TryGetComponent<FarmUnit>(out FarmUnit farmUnit);

        if (dpsUnit != null)
        {
            prefabToShow.name = dpsUnit.unitName;
            costText.text = dpsUnit.cost.ToString() + " $";
        }
        else if (farmUnit != null)
        {
            prefabToShow.name = farmUnit.unitName;

            costText.text = farmUnit.cost.ToString() + " $";
        }
        prefabToShow.SetActive(false);
    }

    void Update()
    {
        prefabToShow.transform.Rotate(Vector3.up * Time.deltaTime * 30f);
    }

    public void ShowUnit()
    {
        // Hide all other unit displays
        foreach (var displayUnit in FindObjectsByType<DisplayUnits>(FindObjectsSortMode.None))
        {
            if (displayUnit != this && displayUnit.isShowing)
            {
                displayUnit.prefabToShow.SetActive(false);
                displayUnit.isShowing = false;
            }
        }

        // If already showing, toggle off
        if (isShowing)
        {
            prefabToShow.SetActive(false);
            isShowing = false;
            return;
        }

        // Reassign the correct unit reference when changing displayed Unit
        dpsUnit = prefabToShow.GetComponent<DpsUnit>();
        farmUnit = prefabToShow.GetComponent<FarmUnit>();

        // Update stats based on new assignment
        if (dpsUnit != null)
        {
            damageText.text = dpsUnit.damage.ToString();
            attackSpeedText.text = dpsUnit.attackCooldown.ToString("F2");
            rangeText.text = dpsUnit.range.ToString();
            costToDeployText.text = dpsUnit.cost + " $";
        }
        else if (farmUnit != null)
        {
            damageText.text = "0";
            attackSpeedText.text = farmUnit.interval.ToString("F2");
            rangeText.text = farmUnit.range.ToString();
            costToDeployText.text = farmUnit.cost + " $";
        }

        unitName.text = prefabToShow.name;
        prefabToShow.SetActive(true);
        isShowing = true;
    }

}
