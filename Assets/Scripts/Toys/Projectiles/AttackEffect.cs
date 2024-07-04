using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f; // Tempo em segundos antes de destruir o efeito

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
