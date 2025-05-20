using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUnits : MonoBehaviour
{
    public GameObject prefabToShow;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI costText;
    public int costToBuy;
    public TextMeshProUGUI costToBuyText;
    public Button buyButton;
    public Image unitIcon;
    public Image unitIconBackground;
    [Header("Stats Text")]
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI costToDeployText;
    private bool isShowing = false;
    private string unitID;
    private bool isUnlocked;
    private DpsUnit dpsUnit;
    private FarmUnit farmUnit;

    void Start()
    {
        prefabToShow.TryGetComponent(out dpsUnit);
        prefabToShow.TryGetComponent(out farmUnit);

        unitID = dpsUnit != null ? dpsUnit.unitName : farmUnit.unitName;
        isUnlocked = SaveManager.Instance.IsUnitUnlocked(unitID);


        if (dpsUnit != null)
        {
            unitID = dpsUnit.unitName;
            //unitIcon.sprite = dpsUnit.icon;
            costText.text = costToBuy + " $";
        }
        else if (farmUnit != null)
        {
            unitID = farmUnit.unitName;
            //unitIcon.sprite = farmUnit.icon;
            costText.text = costToBuy + " $";
        }

        prefabToShow.SetActive(false);

        buyButton.gameObject.SetActive(!isUnlocked);
        buyButton.interactable = !isUnlocked; // Enable button for units not bought yet

        buyButton.onClick.AddListener(() =>
        {
            if (!isShowing) // Ensure only the shown unit can be purchased
            {
                return;
            }

            int cost = costToBuy;

            if (SaveManager.Instance.SpendMoney(cost))
            {
                SaveManager.Instance.UnlockUnit(unitID);
                isUnlocked = true;
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Not enough money");
            }
        });
    }

    void Update()
    {
        if(SaveManager.Instance.CurrentSave.playerMoney < costToBuy)
        {
            costText.color = Color.red;
        }
        else
        {
            costText.color = Color.white;
        }

        if (isUnlocked)
        {
            unitIconBackground.color = Color.white;
            costText.gameObject.SetActive(false);
        }
        else
        {
            unitIconBackground.color = Color.grey;
            costText.gameObject.SetActive(true);
        }

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

        // Update stats for the selected unit
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

        buyButton.gameObject.SetActive(!isUnlocked);
        buyButton.interactable = !isUnlocked; // Enable button for units not bought yet
        costToBuyText.text = costToBuy + " $";

        unitName.text = prefabToShow.name;
        prefabToShow.SetActive(true);
        isShowing = true;
    }

}
