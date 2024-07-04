using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    private Transform target;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float bulletsPerSecond = 10f; // bullets per second
    [SerializeField] private float shootingInterval = 3f; // interval between each shooting burst
    private bool isShooting = false;

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
            if (!isShooting)
            {
                StartCoroutine(ShootBullets());
            }
        }
    }

    private IEnumerator ShootBullets()
    {
        isShooting = true;
        int bulletsToFire = 4;
        float timeBetweenBullets = 0.1f;

        for (int i = 0; i < bulletsToFire; i++)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenBullets);
        }

        yield return new WaitForSeconds(shootingInterval);
        isShooting = false;
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
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
