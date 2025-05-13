using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;

public class UnitDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject unitGhostPrefab;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;
    public TextMeshProUGUI costText;
    private GameObject ghostInstance;
    private bool isDragging = false;
    void Start()
    {
        if(unitGhostPrefab.GetComponent<UnitGhost>().actualUnitPrefab.TryGetComponent<DpsUnit>(out DpsUnit dpsUnit))
        {
            costText.text = dpsUnit.cost + " $";
        }
        else if(unitGhostPrefab.GetComponent<UnitGhost>().actualUnitPrefab.TryGetComponent<FarmUnit>(out FarmUnit farmUnit))
        {
            costText.text = farmUnit.cost + " $";
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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

                if (IsValidPlacement(worldPos))
                {
                    SetGhostMaterial(validPlacementMaterial);
                }
            }
            else
            {
                SetGhostMaterial(invalidPlacementMaterial);
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
                return hit.point + Vector3.up * 5;
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
