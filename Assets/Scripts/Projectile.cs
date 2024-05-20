using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy projectileTarget;

    [SerializeField] private float projectileSpeed;

    private Tower parentTower;

    void Start()
    {

    }

    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower parentTower)
    {
        this.projectileTarget = parentTower.Target;
        this.parentTower = parentTower;
    }

    public void MoveToTarget()
    {
        if(projectileTarget != null && projectileTarget.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, projectileTarget.transform.position, Time.deltaTime * projectileSpeed);

            Vector2 dir = projectileTarget.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        }
        else if(!projectileTarget.IsActive)
        {
            Destroy(gameObject);
        }
    }
}
