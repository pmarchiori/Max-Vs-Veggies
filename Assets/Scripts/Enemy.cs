using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    // public void Spawn()
    // {
    //     transform.position = LevelManager.Instance.PointA.transform.position;
    // }
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;


    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    [SerializeField] private EnemySpawner enemySpawner;

    public bool IsActive = true;

    private void Start()
    {
        target = StageManager.main.path[pathIndex];
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Update()
    {
        if(Vector2.Distance(target.position, transform.position) <= 0.1f) //checks if the enemy isnt already on his target position
        {
            pathIndex++; //changes the current target for the enemy movement

            if(pathIndex == StageManager.main.path.Length) //checks if the enemy has reached the last target on his path
            {
                EnemySpawner.onEnemyKilled.Invoke(); //invokes the event "onEnemyKilled" from the EnemySpawner script
                enemySpawner.DecreaseLife(); //invokes the function "DecreaseLife" from EnemySpawner script
                IsActive = false;
                Destroy(gameObject); //destroys the enemy game object
                return;
            }
            else
            {
                target = StageManager.main.path[pathIndex]; //if the enemy hasnt reached last target, proceeds to next target
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized; //gets the direction the enemy should move

        rb.velocity = direction * moveSpeed; //moves the enemy
    }
}
