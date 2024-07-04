using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : MonoBehaviour
{
[Header("References")]
[SerializeField] private LayerMask enemyMask;
[SerializeField] private GameObject attackEffectPrefab;
private Transform target;

[Header("Attributes")]
[SerializeField] private float targetingRange ;
[SerializeField] private float attackRange; // Novo campo para o tamanho do raio do ataque
[SerializeField] private float aps; // attacks per second
[SerializeField] private int damage; // damage per attack
private float timeUntilAttack;

private void Update()
{
    if (target == null)
    {
        FindTarget();
        return;
    }

    if (!CheckTargetIsInRange())
    {
        target = null;
    }
    else
    {
        timeUntilAttack += Time.deltaTime;

        if (timeUntilAttack >= 1f / aps)
        {
            PerformAreaAttack();
            timeUntilAttack = 0f;
        }
    }
}

private void Start()
{
    timeUntilAttack = 1f / aps;
}

private void PerformAreaAttack()
{
    if (target == null)
    {
        return;
    }

    // Instantiate the attack effect at the target's position
    Instantiate(attackEffectPrefab, target.position, Quaternion.identity);

    // Use attackRange for the area attack radius
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.position, attackRange, enemyMask);

    foreach (Collider2D enemy in hitEnemies)
    {
        EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }
}

private void FindTarget()
{
    // This method finds the closest target within the targeting range
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

    if (hits.Length > 0)
    {
        target = hits[0].transform; // Assigning the first enemy hit as the target
    }
    else
    {
        target = null;
    }
}

private bool CheckTargetIsInRange()
{
    return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
}

private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, targetingRange);

    // Desenha o raio do ataque em área
    if (target != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, attackRange);
    }
}

}
