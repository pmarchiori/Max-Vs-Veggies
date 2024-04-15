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
        Currency = 5;
    }

    void Update()
    {
        HandleEsc();
    }

    public void PickTower(TowerBtn towerBtn)
    {
        this.ClickedTowerBtn = towerBtn;
        Hover.Instance.Activate(towerBtn.Sprite);
    }

    public void BuyTower()
    {  
        Hover.Instance.Deactivate();
    }

    private void HandleEsc()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }
}
