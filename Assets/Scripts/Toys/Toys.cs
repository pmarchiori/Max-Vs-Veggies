using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toys : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int Price { get; set; }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Select()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }
}
