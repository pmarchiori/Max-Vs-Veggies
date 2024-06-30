using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    private Transform target;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float bps; //bullets per second
    private float timeUntilFire;

    private void Start()
    {
        timeUntilFire = 1f / bps - 0.01f;
    }

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
            timeUntilFire += Time.deltaTime;

            if(timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        Vector2 directionToTarget = (target.position - firingPoint.position).normalized;
        
        // Bullet no centro
        InstantiateBullet(directionToTarget);

        // Bullet à esquerda
        Vector2 leftDirection = Quaternion.Euler(0, 0, 30) * directionToTarget; // Gire 30 graus à esquerda
        InstantiateBullet(leftDirection);

        // Bullet à direita
        Vector2 rightDirection = Quaternion.Euler(0, 0, -30) * directionToTarget; // Gire 30 graus à direita
        InstantiateBullet(rightDirection);
    }

    private void InstantiateBullet(Vector2 direction)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        NinjaProjectile projectileScript = bulletObj.GetComponent<NinjaProjectile>();
        projectileScript.SetDirection(direction);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if(hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}
