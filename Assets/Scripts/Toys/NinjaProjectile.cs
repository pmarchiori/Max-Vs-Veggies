using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage;

    private Vector2 direction;

    public void SetDirection(Vector2 _direction)
    {
        direction = _direction.normalized;
    }

    private void Start()
    {
        if (direction != Vector2.zero)
        {
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            // Se a direção não for válida, destrua a bullet ou defina uma direção padrão
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }

        if (other.collider.CompareTag("LevelCollider"))
        {
            Destroy(gameObject);
        }
    }
}
