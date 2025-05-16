using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UnitDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject unitGhostPrefab;
    public TextMeshProUGUI costText;
    private GameObject ghostInstance;
    public Image unitIcon;
    private bool isDragging = false;
    void Start()
    {
        if (unitGhostPrefab.GetComponent<UnitGhost>().actualUnitPrefab.TryGetComponent<DpsUnit>(out DpsUnit dpsUnit))
        {
            costText.text = dpsUnit.cost + " $";
        }
        else if (unitGhostPrefab.GetComponent<UnitGhost>().actualUnitPrefab.TryGetComponent<FarmUnit>(out FarmUnit farmUnit))
        {
            costText.text = farmUnit.cost + " $";
        }
    }

    void Update()
    {

        
        if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }

        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.TryGetComponent(out DpsUnit dps))
                    dps.ShowStats();
                else if (hit.collider.TryGetComponent(out FarmUnit farm))
                    farm.ShowStats();
            }
        }

        if (unitGhostPrefab.GetComponent<UnitGhost>().actualUnitPrefab.TryGetComponent<DpsUnit>(out DpsUnit dpsUnit))
        {
            if(dpsUnit.cost > GameManager.Instance.playerMoney)
            {
                costText.color = Color.red;
            }
            else
            {
                costText.color = Color.white;
            }

            if (SaveManager.Instance.IsUnitUnlocked(dpsUnit.unitName))
            {
                unitIcon.color = Color.white;
            }
            else
            {
                unitIcon.color = Color.red;
            }
        }
        else if (unitGhostPrefab.GetComponent<UnitGhost>().actualUnitPrefab.TryGetComponent<FarmUnit>(out FarmUnit farmUnit))
        {
            if(farmUnit.cost > GameManager.Instance.playerMoney)
            {
                costText.color = Color.red;
            }
            else
            {
                costText.color = Color.white;
            }

            if (SaveManager.Instance.IsUnitUnlocked(farmUnit.unitName))
            {
                unitIcon.color = Color.white;
            }
            else
            {
                unitIcon.color = Color.red;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        string unitID = "";
        GameObject actualUnit = unitGhostPrefab.GetComponent<UnitGhost>().actualUnitPrefab;
        
        if (actualUnit.TryGetComponent(out DpsUnit dps))
            unitID = dps.unitName;
        else if (actualUnit.TryGetComponent(out FarmUnit farm))
            unitID = farm.unitName;


        if (!SaveManager.Instance.IsUnitUnlocked(unitID))
        {
            Debug.Log("Unit is locked and cannot be dragged.");
            return;
        }

        ghostInstance = Instantiate(unitGhostPrefab);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (ghostInstance != null)
        {
            isDragging = true;
            Vector3 worldPos = GetWorldMousePosition();
            if (worldPos != Vector3.negativeInfinity)
            {
                ghostInstance.transform.position = worldPos;
            }
         
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (ghostInstance != null)
        {
            Vector3 placePos = GetWorldMousePosition();
            if (IsValidPlacement(placePos))
            {
                ghostInstance.GetComponent<UnitGhost>().ConfirmPlacement();
            }
            else
            {
                Destroy(ghostInstance);
            }
        }
    }

    public void CancelPlacement()
    {
        if (ghostInstance != null)
        {
            isDragging = false;
            Destroy(ghostInstance);
        }
    }

    Vector3 GetWorldMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Background = valid placement
            if (hit.collider.gameObject.name == "Background")
            {
                return hit.point + Vector3.up * 10;
            }

            // Unit interactions
            if (hit.collider.TryGetComponent(out DpsUnit dpsUnit))
            {
                dpsUnit.ShowStats();
                Debug.Log("DpsUnit detected");
            }
            else if (hit.collider.TryGetComponent(out FarmUnit farmUnit))
            {
                farmUnit.ShowStats();
                Debug.Log("FarmUnit detected");
            }

            return Vector3.negativeInfinity; // 👈 Not valid for placement
        }

        return Vector3.negativeInfinity; // 👈 No raycast hit at all
    }


    bool IsValidPlacement(Vector3 pos)
    {
        // Add custom placement rules here
        return true;
    }

    void SetGhostMaterial(Material material)
    {
        Renderer renderer = ghostInstance.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}
