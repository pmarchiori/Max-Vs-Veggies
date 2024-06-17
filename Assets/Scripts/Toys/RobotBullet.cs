using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage;

    private Transform target;
    private int enemyHitCount = 0;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if(!target) return;
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if(other.collider.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
            enemyHitCount++;
            
            if(enemyHitCount >= 2)
            {
                Destroy(gameObject);
            }
            else
            {
                target = FindNextEnemy(other.transform);
            }
        }

        if(other.collider.CompareTag("LevelCollider"))
        {
            Destroy(gameObject);
        }
    }

    private Transform FindNextEnemy(Transform currentEnemy)
    {
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(enemy.transform == currentEnemy) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}
