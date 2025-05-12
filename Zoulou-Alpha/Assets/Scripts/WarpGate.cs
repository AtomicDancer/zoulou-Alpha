using UnityEngine;

public class WarpGate : MonoBehaviour
{
    public int stocks; // Current number of stocks

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void TakeDamage(int stocksDamage)
    {
        stocks -= stocksDamage; // Reduce current health by the damage amount
        if (stocks <= 0)
        {
            //Mission failed
        }
    }
}
