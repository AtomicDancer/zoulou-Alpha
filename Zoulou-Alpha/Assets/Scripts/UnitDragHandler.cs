using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject unitGhostPrefab;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;
    private GameObject ghostInstance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        ghostInstance = Instantiate(unitGhostPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostInstance != null)
        {
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

    Vector3 GetWorldMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject.name == "Background")
            {
                return hit.point + Vector3.up * 5; 
            }
            else
            {
                return Vector3.negativeInfinity;
            }
        }
        
        return hit.point + Vector3.up * 5;
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
