using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public TowerBtn ClickedTowerBtn  { get; private set; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PickTower(TowerBtn towerBtn)
    {
        this.ClickedTowerBtn = towerBtn;
        Hover.Instance.Activate(towerBtn.Sprite);
    }

    public void BuyTower()
    {
        ClickedTowerBtn = null;
    }
}
