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
    private Toys selectedTower; //current selected tower
    public TowerBtn ClickedTowerBtn  { get; set; }
    [SerializeField] private GameObject gameWonMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject upgradePanel;        
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI speedButtonText;
    //[SerializeField] private Button changeSpeedButton;

    [Header("Tower Upgrades References")]
    [SerializeField] private GameObject soldierUpgradePanel;
    [SerializeField] private GameObject robotUpgradePanel;
    [SerializeField] private GameObject tank;
    [SerializeField] private GameObject sniper;
    [SerializeField] private GameObject mecha;

    [Header("Attributes")]
    [SerializeField] private int currency;
    [SerializeField] private int currencyAddFromWave;
    private bool gameOver = false;
    public event CurrencyChanged Changed; //event that is triggered when the currency changes

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

    private void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        HandleEsc();
        currencyText.text = currency.ToString();
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

    public void SelectTower(Toys toys)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = toys; 
        selectedTower.Select();

        sellText.text = "Sell for: " + (selectedTower.Price / 2).ToString();

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

        soldierUpgradePanel.SetActive(false);
        robotUpgradePanel.SetActive(false);
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

    public void ShowUpgradeMenu()
    {
        if(selectedTower.CompareTag("Soldier"))
        {
            soldierUpgradePanel.SetActive(true);
        }

        if(selectedTower.CompareTag("Robot"))
        {
            robotUpgradePanel.SetActive(true);
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

    public void GameWon()
    {
        gameWonMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void OnCurrencyChanged()
    {
        if(Changed != null)
        {
            Changed();
        }
    }

    public void OnWaveEnded()
    {
        Currency += currencyAddFromWave;
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

    public void ChangeGameSpeed()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 2;
            speedButtonText.text = ">";
        }
        else if(Time.timeScale == 2)
        {
            Time.timeScale = 1;
            speedButtonText.text = ">>>";
        }
    }

    private void DropTower()
    {
        ClickedTowerBtn = null;
        Hover.Instance.Deactivate(); //deactivates the hover icon instance
    }

    #region TOWER UPGRADES
    public void SoldierToTank()
    {
        if(selectedTower != null && Currency > 20)
        {
            Currency -= 20;
            // Store the current position and rotation of the tower to be replaced
            Vector3 position = selectedTower.transform.parent.position;
            Quaternion rotation = selectedTower.transform.parent.rotation;

            // Instantiate the new prefab at the same position and rotation
            GameObject newPrefabInstance = Instantiate(tank, position, rotation);

            Toys newToysComponent = newPrefabInstance.GetComponentInChildren<Toys>();
            if (newToysComponent != null)
            {
               newToysComponent.Price = selectedTower.Price; // Copy necessary data
            }

            // Destroy the current tower and its parent
            Destroy(selectedTower.transform.parent.gameObject);

            // Deselect the old tower
            DeselectTower();
        }
    }

    public void SoldierToSniper()
    {
        if(selectedTower != null && Currency > 20)
        {
            Currency -= 20;
            // Store the current position and rotation of the tower to be replaced
            Vector3 position = selectedTower.transform.parent.position;
            Quaternion rotation = selectedTower.transform.parent.rotation;

            // Instantiate the new prefab at the same position and rotation
            GameObject newPrefabInstance = Instantiate(sniper, position, rotation);

            Toys newToysComponent = newPrefabInstance.GetComponentInChildren<Toys>();
            if (newToysComponent != null)
            {
               newToysComponent.Price = selectedTower.Price; // Copy necessary data
            }

            // Destroy the current tower and its parent
            Destroy(selectedTower.transform.parent.gameObject);

            // Deselect the old tower
            DeselectTower();
        }
    }

    public void RobotToMecha()
    {
        if(selectedTower != null && Currency > 20)
        {
            Currency -= 20;
            // Store the current position and rotation of the tower to be replaced
            Vector3 position = selectedTower.transform.parent.position;
            Quaternion rotation = selectedTower.transform.parent.rotation;

            // Instantiate the new prefab at the same position and rotation
            GameObject newPrefabInstance = Instantiate(mecha, position, rotation);

            Toys newToysComponent = newPrefabInstance.GetComponentInChildren<Toys>();
            if (newToysComponent != null)
            {
               newToysComponent.Price = selectedTower.Price; // Copy necessary data
            }

            // Destroy the current tower and its parent
            Destroy(selectedTower.transform.parent.gameObject);

            // Deselect the old tower
            DeselectTower();
        }
    }

    #endregion
}