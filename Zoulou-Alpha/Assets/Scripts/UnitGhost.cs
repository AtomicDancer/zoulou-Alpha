using UnityEngine;

public class UnitGhost : MonoBehaviour
{
    public GameObject actualUnitPrefab;
    public GameObject rangeVisualPrefab;
    public float unitRange = 3f;
    private GameObject rangeVisual;

    void Start()
    {
        if (actualUnitPrefab.TryGetComponent<DpsUnit>(out DpsUnit dpsUnit))
        {
            unitRange = dpsUnit.range;
        }
        else
        {
            unitRange = 0f;
        }
        rangeVisual = Instantiate(rangeVisualPrefab, transform);
        UpdateRangeVisual();
    }

    void UpdateRangeVisual()
    {
        float diameter = unitRange / 2.5f;
        rangeVisual.transform.localScale = new Vector3(diameter, 0.01f, diameter);
    }

    public void ConfirmPlacement()
    {
        if (actualUnitPrefab.TryGetComponent<FarmUnit>(out FarmUnit farmUnit))
        {
            if (GameManager.Instance.playerMoney >= farmUnit.cost)
            {
                Instantiate(actualUnitPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject); 
                GameManager.Instance.SpendMoney(farmUnit.cost);
            }
            else
            {
                Destroy(gameObject); 
                return;
            }
        }
        else if (actualUnitPrefab.TryGetComponent<DpsUnit>(out DpsUnit dpsUnit))
        {
            if (GameManager.Instance.playerMoney >= dpsUnit.cost)
            {
                Instantiate(actualUnitPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject); 
                GameManager.Instance.SpendMoney(dpsUnit.cost);
            }
            else
            {
                Destroy(gameObject); 
                return;
            }
        }
        {
            Destroy(gameObject); 
        }
    }

}
