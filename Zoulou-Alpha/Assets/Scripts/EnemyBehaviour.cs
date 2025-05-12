using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public float speed = 5f; // Speed of the enemy
    public int stocksDamage = 1; // Damage inflicted by the enemy
    public Transform target; // Target to follow


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("WarpGate").transform;
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("WarpGate"))
        {
            Destroy(gameObject); 
            //  InflictDamage(stocksDamage, other);
        }
    }

    public void InflictDamage(int stocksDamage, Collider other)
    {
        // other.TryGetComponent(out WarpGate warpGate);
        // if (warpGate != null)
        // {
        //     warpGate.TakeDamage(damage);
        //     Destroy(gameObject); 
        //     //Add Feedbacks
        // }
    }
}
