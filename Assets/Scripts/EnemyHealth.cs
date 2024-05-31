using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints;
    [SerializeField] int currencyWorth;

    private bool isKilled = false;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if(hitPoints <= 0 && !isKilled)
        {
            EnemySpawner.onEnemyKilled.Invoke();
            gameManager.IncreaseCurrency(currencyWorth);
            isKilled = true;
            Destroy(gameObject);
        }
    }
}
