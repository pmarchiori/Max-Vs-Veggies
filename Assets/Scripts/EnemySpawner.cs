using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private TextMeshProUGUI waveCounterText;
    [SerializeField] private GameObject NextWaveButton;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 6;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 1f;
    [SerializeField] private float diffScalingFactor = 0.75f;
    [SerializeField] private float epsCap = 10f;

    [Header("Events")]
    public static UnityEvent onEnemyKilled = new UnityEvent();

    [SerializeField] private int currentWave = 0;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; //enemies per second
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyKilled.AddListener(EnemyKilled);
    }

    private void Start()
    {
        //StartCoroutine(StartWave());
    }

    private void Update()
    {
        waveCounterText.text = "Wave: " + currentWave;

        if(!isSpawning)
        {
            return;
        }
        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyKilled()
    {
        enemiesAlive--;
    }

    public void StartWaveFromButton()
    {
        NextWaveButton.SetActive(false);
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        currentWave++;

        yield return new WaitForSeconds(0.5f);

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        NextWaveButton.SetActive(true);

        //currentWave++;

        //StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, StageManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, diffScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, diffScalingFactor), 0f, epsCap);
    }
}
