using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void CurrencyChanged();  //delegate for the currency changed event

public class GameManager : Singleton<GameManager> 
{
    [Header("References")]
    private Turret selectedTower; //current selected tower
    public TowerBtn ClickedTowerBtn  { get; set; }
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject upgradePanel;        
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Attributes")]
    private bool gameOver = false;
    public event CurrencyChanged Changed; //event that is triggered when the currency changes
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
            //this.currencyText.text = value.ToString() + "$";

            OnCurrencyChanged();
        }
    }

    //public ObjectPool Pool { get; set; }

    // private void Awake()
    // {
    //     Pool = GetComponent<ObjectPool>();
    // }

    void Start()
    {
        Currency = 10;
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

    public void SellTower()
    {
        if(selectedTower != null)
        {
            Currency += selectedTower.Price / 2;

            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;

            Destroy(selectedTower.transform.parent.gameObject);

            DeselectTower();
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

        sellText.text = "+ " + (selectedTower.Price / 2).ToString();

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
            if(selectedTower == null && !Hover.Instance.IsVisible)
            {    
                ShowPauseMenu();
            }
            else if(Hover.Instance.IsVisible)
            {
                DropTower();
            }
            else if(selectedTower != null)
            {
                DeselectTower();
            }
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

    public void OnCurrencyChanged()
    {
        if(Changed != null)
        {
            Changed();
        }
    }

    public void ShowPauseMenu()
    {
        if(optionsMenu.activeSelf)
        {
            HideOptionsMenu();
        }
        else
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            if(!pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    public void ShowOptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void HideOptionsMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    private void DropTower()
    {
        ClickedTowerBtn = null;
        Hover.Instance.Deactivate(); //deactivates the hover icon instance
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
