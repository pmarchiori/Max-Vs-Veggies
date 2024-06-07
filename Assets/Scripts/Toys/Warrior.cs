using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Warrior : MonoBehaviour
{
     [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject attackEffectPrefab;
    private Transform target;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float aps = 1f; //attacks per second
    [SerializeField] private int damage = 10; //damage per attack
    private float timeUntilAttack;

    private void Update()
    {
        if(target == null)
        {
            FindTarget();
            return;
        }

        if(!CheckTargetIsInRange())
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
        timeUntilAttack = 2.99f;
    }

    private void PerformAreaAttack()
    {
        Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

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
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}

