using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnEnemyKilled();
    }
}