using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType { FullAOE, Single }

public class DpsUnit : BaseUnit
{
    public float range = 3f;
    public int damage = 10;
    public float attackCooldown = 1f;
    public AttackType attackType;
    private float timer;
    public LineRenderer singleAttackFx;
    public bool canShoot;

    void Start()
    {
        rangeVisualPrefab.SetActive(false);
        rangeVisualPrefab.transform.localScale = new Vector3(range / 2.5f , 0.01f, range / 2.5f);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            var enemies = GetEnemiesInRange();
            if (enemies.Count > 0 && canShoot)
            {
                ExecuteAttack(enemies);
                timer = attackCooldown;
            }
        }
    }

    private EnemyBehaviour currentTarget;
    void ExecuteAttack(List<EnemyBehaviour> enemies)
    {
        switch (attackType)
        {
            case AttackType.FullAOE:
                foreach (var e in enemies) e.TakeDamage(damage);
                break;

            case AttackType.Single:
                if (currentTarget == null || !enemies.Contains(currentTarget))
                {
                    if (currentTarget != null && enemies.Contains(currentTarget))
                    {
                        int currentIndex = enemies.IndexOf(currentTarget);
                        currentTarget = enemies[(currentIndex + 1) % enemies.Count];
                    }
                    else
                    {
                        currentTarget = enemies[0];
                    }
                }

                // Create a line renderer effect for the single target attack
                var attackFxInstance = Instantiate(singleAttackFx, transform.position, Quaternion.identity);
                attackFxInstance.SetPosition(0, transform.position);
                attackFxInstance.SetPosition(1, currentTarget.transform.position);
                Destroy(attackFxInstance.gameObject, 0.2f);
                Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);
                currentTarget.TakeDamage(damage);

                break;
        }
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
