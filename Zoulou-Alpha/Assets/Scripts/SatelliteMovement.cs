using UnityEngine;

public class SatelliteMovement : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 20 * Time.deltaTime);
    }
}
