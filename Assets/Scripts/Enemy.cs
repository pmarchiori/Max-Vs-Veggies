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

    private void Start()
    {
        target = StageManager.main.path[pathIndex];
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Update()
    {
        if(Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if(pathIndex == StageManager.main.path.Length)
            {
                EnemySpawner.onEnemyKilled.Invoke();
                enemySpawner.DecreaseLife();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = StageManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }
}
