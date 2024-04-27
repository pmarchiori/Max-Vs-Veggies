using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI f;
    [SerializeField] private TextMeshProUGUI g;
    [SerializeField] private TextMeshProUGUI h;

    public TextMeshProUGUI F
    {
        get
        {
            f.gameObject.SetActive(true);
            return f;
        }

        set
        {
            this.f = value;
        }
    } 

    public TextMeshProUGUI G
    {
        get
        {
            g.gameObject.SetActive(true);
            return g;
        }

        set
        {
            this.g = value;
        }
    } 

    public TextMeshProUGUI H
    {
        get
        {
            h.gameObject.SetActive(true);
            return h;
        }

        set
        {
            this.h = value;
        }
    } 
}
