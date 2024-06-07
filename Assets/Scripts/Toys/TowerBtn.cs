using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab; //the prefab that this button will spawn

    [SerializeField] private Sprite sprite; //the tower's sprite

    [SerializeField] private int price;

    [SerializeField] private TextMeshProUGUI priceTxt;

    //property for accessing the button's prefab
    public GameObject TowerPrefab 
    {
        get
        {
            return towerPrefab;
        }
    }

    //property for accessing the tower's sprite
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public int Price
    {
        get
        {
            return price;
        }
    }

    private void Start()
    {
        priceTxt.text = price + "$";

        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }

    private void Update()
    {
        PriceCheck();
    }

    private void PriceCheck()
    {
        if(price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceTxt.color = Color.grey;
        }
    }
}