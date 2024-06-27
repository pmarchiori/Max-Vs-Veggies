using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFITower : MonoBehaviour
{
    private Coroutine addCurrencyCoroutine;
    [SerializeField] private int currencyAddition;
    [SerializeField] private float seconds;

    private void OnEnable()
    {
        // Inscreva-se nos eventos
        EnemySpawner.onWaveStarted.AddListener(StartAddingCurrency);
        EnemySpawner.onWaveEnded.AddListener(StopAddingCurrency);
    }

    private void OnDisable()
    {
        // Cancele a inscrição nos eventos
        EnemySpawner.onWaveStarted.RemoveListener(StartAddingCurrency);
        EnemySpawner.onWaveEnded.RemoveListener(StopAddingCurrency);
    }

    private void StartAddingCurrency()
    {
        addCurrencyCoroutine = StartCoroutine(AddCurrencyPeriodically());
    }

    private void StopAddingCurrency()
    {
        if (addCurrencyCoroutine != null)
        {
            StopCoroutine(addCurrencyCoroutine);
        }
    }

    private IEnumerator AddCurrencyPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            GameManager.Instance.Currency += currencyAddition;
        }
    }
}
