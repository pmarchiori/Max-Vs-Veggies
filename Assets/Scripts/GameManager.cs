using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> 
{
    public TowerBtn ClickedTowerBtn  { get; set; }
 
    [SerializeField] private TextMeshProUGUI currencyText;

    //public ObjectPool Pool { get; set; }

    [SerializeField] private int currency;

    private bool gameOver = false;

    [SerializeField] private GameObject gameOverMenu;

    private Turret selectedTower; //current selected tower

    [SerializeField] private GameObject upgradePanel;

    [SerializeField] private TextMeshProUGUI sellText;

    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value;
            //this.currencyText.text = value.ToString() + "$";
        }
    }

    // private void Awake()
    // {
    //     Pool = GetComponent<ObjectPool>();
    // }

    void Start()
    {
        Currency = 50;
    }

    void Update()
    {
        HandleEsc();
        currencyText.text = currency.ToString() + "$";
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
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

    public void SelectTower(Turret turret)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = turret; 
        selectedTower.Select();

        //sellText.text = "+ " + (selectedTower.price)

        upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        
        upgradePanel.SetActive(false);

        selectedTower = null; //remove the reference to the tower 
    }

    private void HandleEsc() //handles the escape key
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate(); //deactivates the hover icon instance
        }
    }

    public void GameOver()
    {
        if(!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitLevel()
    {
        
    }

    // private void StartWave()
    // {
    //     StartCoroutine(SpawnWave());
    // }

    // private IEnumerator SpawnWave()
    // {
    //     int enemyIndex = Random.Range(0,3);

    //     string type = string.Empty;

    //     switch(enemyIndex)
    //     {
    //         case 0:
    //             type = "Mob_01";
    //             break;
    //         case 1:
    //             type = "Mob_02";
    //             break;
    //         case 2:
    //             type = "Mob_03";
    //             break;
    //     }

    //     Enemy enemy = Pool.GetObject(type).GetComponent<Enemy>(); //requests enemy from the pool
    //     //enemy.Spawn();
    //     yield return new WaitForSeconds(2.5f);
    // }
}
