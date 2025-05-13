using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public float speed = 5f; // Speed of the enemy
    public int stocksDamage = 1; // Damage inflicted by the enemy
    public int health = 1; // Health of the enemy
    public Transform target; // Target to follow


    void Start()
    {
        target = FindTargetInFront();
    }

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        if (transform.position == target.position)
        {
            Destroy(gameObject);
            InflictDamage(stocksDamage, target.GetComponent<Collider>());
        }
    }

    public void InflictDamage(int stocksDamage, Collider other)
    {
        other.TryGetComponent(out WarpGate warpGate);
        if (warpGate != null)
        {
            warpGate.TakeDamage(stocksDamage);
            Destroy(gameObject); 
            //Add Feedbacks
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            //Add Feedbacks
        }
    }

    Transform FindTargetInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Water")))
        {
            if (hit.collider.CompareTag("WarpGate"))
            {
                target = hit.transform;
                return target;
            }
        }
        return null;
    }

}
