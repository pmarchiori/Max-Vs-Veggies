using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TowerBtn ClickedTowerBtn  { get; set; }
 
    [SerializeField] private TextMeshProUGUI currencyText;

    [SerializeField] private int currency;

    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value;
            this.currencyText.text = value.ToString() + "$";
        }
    }

    void Start()
    {
        Currency = 10;
    }

    void Update()
    {
        HandleEsc();
    }

    public void PickTower(TowerBtn towerBtn) //pick the respective tower of the button pressed
    {
        if(Currency >= towerBtn.Price)
        {
            this.ClickedTowerBtn = towerBtn; //stores the clicked button
            Hover.Instance.Activate(towerBtn.Sprite); //activates the hover icon for the tower placement
        }
        
    }

    public void BuyTower()
    {  
        if(Currency >= ClickedTowerBtn.Price)
        {
            Currency -= ClickedTowerBtn.Price;

            Hover.Instance.Deactivate();
        }
    }

    private void HandleEsc() //handles the escape key
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate(); //deactivates the hover icon instance
        }
    }
}
