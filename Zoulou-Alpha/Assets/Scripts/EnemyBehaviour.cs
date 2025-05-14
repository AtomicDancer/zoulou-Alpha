using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public WaypointPath path; // Assigned when spawned
    public float speed = 5f; // Speed of the enemy
    public int stocksDamage = 1; // Damage inflicted by the enemy
    public float health = 1; 
    public int maxHealth = 1;
    public int MoneyOnDeath = 50; // Money given to the player when the enemy dies
    private int currentWaypointIndex = 0;
    public Image healthBarPrefab;

    void Start()
    {
        healthBarPrefab.GetComponentInParent<Canvas>().worldCamera = Camera.main;
        health = maxHealth; 
        healthBarPrefab.fillAmount = health / maxHealth;
    }
    void Update()
    {
        healthBarPrefab.transform.LookAt(Camera.main.transform);

        if (path == null || currentWaypointIndex >= path.WaypointCount)
            return;

        Transform targetWaypoint = path.GetWaypoint(currentWaypointIndex);
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(targetWaypoint.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

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
        InflictDamage(stocksDamage);
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
        healthBarPrefab.fillAmount = health / maxHealth;
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.AddMoney(MoneyOnDeath); 
        }
    }

}
