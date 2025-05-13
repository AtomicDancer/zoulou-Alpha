using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttackType { FullAOE, Cone, Single }

public class DpsUnit : BaseUnit
{
    public float range = 3f;
    public int damage = 10;
    public float attackCooldown = 1f;
    public AttackType attackType;
    public float coneAngle = 45f;
    private float timer;

    void Start()
    {
        rangeVisualPrefab.SetActive(false);
        rangeVisualPrefab.transform.localScale = new Vector3(range / 2.5f, 0.01f, range / 2.5f);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            var enemies = GetEnemiesInRange();
            if (enemies.Count > 0)
            {
                ExecuteAttack(enemies);
                timer = attackCooldown;
            }
        }
    }

    void ExecuteAttack(List<EnemyBehaviour> enemies)
    {
        switch (attackType)
        {
            case AttackType.FullAOE:
                foreach (var e in enemies) e.TakeDamage(damage);
                break;

            case AttackType.Cone:
                foreach (var e in enemies)
                {
                    Vector3 dir = (e.transform.position - transform.position).normalized;
                    float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
                    if (Mathf.Abs(angle) <= coneAngle)
                        e.TakeDamage(damage);
                }
                break;

            case AttackType.Single:
                var target = enemies[0];
                foreach (var e in enemies)
                {
                    if (Vector3.Distance(transform.position, e.transform.position) < Vector3.Distance(transform.position, target.transform.position))
                        target = e;
                }
                target.TakeDamage(damage);
                // Spawn Fx like a line renderer
                break;
        }
        Debug.Log($"Attacked {enemies.Count} enemies in range.");
    }

    public void ShowStats()
    {
        rangeVisualPrefab.SetActive(true);
        //Add Stats Canvas
    }

    public void HideStats()
    {
        rangeVisualPrefab.SetActive(false);
        //Remove Stats Canvas
    }

    List<EnemyBehaviour> GetEnemiesInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Enemy"));
        return hits.Select(h => h.GetComponent<EnemyBehaviour>()).Where(e => e != null).ToList();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
