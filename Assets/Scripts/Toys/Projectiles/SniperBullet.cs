using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage;

    private Transform target;
    private Vector2 direction;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Start()
    {
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            // Se o alvo não for válido, destrua a bullet ou defina uma direção padrão
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
            //Destroy(gameObject);
        }

        if (other.collider.CompareTag("LevelCollider"))
        {
            Destroy(gameObject);
        }
    }
}
