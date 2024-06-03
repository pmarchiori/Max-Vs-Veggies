// using System.Collections;
// using System.Collections.Generic;
// using JetBrains.Annotations;
// using Unity.Mathematics;
// using Unity.VisualScripting;
// using UnityEngine;

// public class Projectile : MonoBehaviour
// {
//     private Enemy projectileTarget;

//     [SerializeField] private float projectileSpeed;

//     private Tower parentTower;

//     void Start()
//     {

//     }

//     void Update()
//     {
//         MoveToTarget();
//     }

//     public void Initialize(Tower parentTower)
//     {
//         this.projectileTarget = parentTower.Target;
//         this.parentTower = parentTower;
//     }

//     public void MoveToTarget()
//     {
//         if(projectileTarget != null && projectileTarget.IsActive)
//         {
//             transform.position = Vector3.MoveTowards(transform.position, projectileTarget.transform.position, Time.deltaTime * projectileSpeed); //move towards enemy

//             Vector2 dir = projectileTarget.transform.position - transform.position; //calculates direction of the projectile

//             float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; //calcules angle of the projectile

//             transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward); //sets the rotation based on the angle
//         }
//         else if(!projectileTarget.IsActive)
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if(other.tag == "Enemy")
//         {
//             //Destroy(gameObject);
//         }
//     }
// }
