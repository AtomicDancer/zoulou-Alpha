using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public WaypointPath path; // Assigned when spawned
    public float speed = 5f; // Speed of the enemy
    public int stocksDamage = 1; // Damage inflicted by the enemy
    public int health = 1; // Health of the enemy
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (path == null || currentWaypointIndex >= path.WaypointCount)
            return;

        Transform targetWaypoint = path.GetWaypoint(currentWaypointIndex);
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(targetWaypoint); 

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distance < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= path.WaypointCount)
            {
                OnReachDestination();
            }
        }
    }

    void OnReachDestination()
    {
        // Damage the warp gate or whatever chaos you wish
        Debug.Log("Enemy reached the end!");
        Destroy(gameObject);
    }

    public void InflictDamage(int stocksDamage)
    {
        WarpGate warpGate = FindFirstObjectByType<WarpGate>();
        warpGate.TakeDamage(stocksDamage);
        Destroy(gameObject); 
        //Add Feedbacks
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.AddMoney(50); 
        }
    }

}
