using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreparingToPlayUI : MonoBehaviour
{
    [Header("Acquired Heroes")]
    [Space(5)]
    [SerializeField] private LevelsMapAcquirdHero acquirdHeroPrefab;
    [SerializeField] private LevelsMapAcquirdTower acquiredTowerPrefab;
    [SerializeField] private GameObject acquiredItemsContainer;
    [SerializeField] private Transform contentParentTransform;
    [Space(10)]
    [SerializeField] private List<SelectedHeroUI> selectedHeroUIs;
    [SerializeField] private List<SelectedTowerUI> selectedTowerUIs;
    [SerializeField] private Button levelStartButton;
    [SerializeField] private TextMeshProUGUI energyCostText;
    [Space(10)]
    [SerializeField] private LevelsMapDataHandler levelsMapDataHandler;
    [SerializeField] private HeroesDataHandler heroesDataHandler;
    [SerializeField] private TowersDataHandler towersDataHandler;
    [SerializeField] private List<Button> quitButtons;
    [SerializeField] private GameObject preparingToPlayPanel;
    [SerializeField] private AllTowersBlueprintSO allTowers;
    [SerializeField] private HeroesFullBlueprintSO allHeroes;

    private List<LevelsMapAcquirdHero> acquiredHeroes = new();  
    private List<LevelsMapAcquirdTower> acquirdTowers = new();

    private List<int> selectedAcquiredTowersByIndex;
    private List<int> selectedAcquiredHeroesByIndex;

    private LevelsMapAcquirdHero currentSelectedAcquiredHero;
    private LevelsMapAcquirdTower currentSelectedAcquiredTower;

    private SelectedHeroUI currentSelectedHeroUI;
    private SelectedTowerUI currentSelectedTowerUI;


    private void Awake()
    {        
        selectedAcquiredHeroesByIndex = heroesDataHandler.SelectedHeroesByIndex;
        selectedAcquiredTowersByIndex = towersDataHandler.SelectedTowersByIndex;

        for(int i = 0; quitButtons.Count > i; i++)
        {
            quitButtons[i].onClick.AddListener(OnQuit);
        }

        levelStartButton.onClick.AddListener(OnLevelStartButtonClick);
        energyCostText.text = levelsMapDataHandler.PlayCost.ToString();

        CreateAcquirdTowers();
        CreateAcquiredHeroes();

        acquiredItemsContainer.SetActive(false);
    }

    private void Start()
    {
        HideAcquiredItemsContainer();
    }

    public void OnLevelStartButtonClick()
    {
        if (levelsMapDataHandler.TryToStartPlay())
        {
            heroesDataHandler.SaveSelectedHeroesBeforeStartPlayLevel();
            towersDataHandler.SaveSelectedTowersBeforeStartPlayLevel();
        }
    }

    private void OnQuit()
    {
        HideAcquiredItemsContainer();
        preparingToPlayPanel.SetActive(false);
    }

    public void HideAcquiredItemsContainer()
    {
        acquiredItemsContainer.SetActive(false);

        CleanAllAcquiredItems();

        if (currentSelectedTowerUI)
        {
            currentSelectedTowerUI.UnSelect();
            currentSelectedTowerUI = null;
        }
        if (currentSelectedHeroUI)
        {
            currentSelectedHeroUI.UnSelect();
            currentSelectedHeroUI = null;
        }
    }

    public void ShowAcquiredItemsContainer()
    {
        acquiredItemsContainer.SetActive(true);
    }

    private void CleanAllAcquiredItems()
    {
        CleanAcquirdHeros();
        CleanAcquirdTowers();
    }



    #region Toweres

    public void SetCurrentSelectedTowerUI(SelectedTowerUI selectedTowerUI)
    {
        CleanAllAcquiredItems();

        ShowAcquiredItemsContainer();

        CreateAcquirdTowers();

        if (currentSelectedTowerUI != null && currentSelectedTowerUI != selectedTowerUI)
        {
            currentSelectedTowerUI.UnSelect();
        }

        currentSelectedTowerUI = selectedTowerUI;
    }

    private void CreateAcquirdTowers()
    {
        if (currentSelectedHeroUI != null)
        {
            currentSelectedHeroUI.UnSelect();
            currentSelectedHeroUI = null;
        }

        for (int i = 0; allTowers.AcquiredTowers.Count > i; i++)
        {
            acquirdTowers.Add(Instantiate(acquiredTowerPrefab, contentParentTransform, true));
            acquirdTowers[i].SetPreparingToPlay(this);
            acquirdTowers[i].SetParams(allTowers.AcquiredTowers[i].TowerUpgrades[0], i);
        }

        for (int i = 0; i < selectedTowerUIs.Count; i++)
        {
            selectedTowerUIs[i].Construct(this);
            selectedTowerUIs[i].SetParams(acquirdTowers[selectedAcquiredTowersByIndex[i]].TowerSO);
        }
    }

    private void CleanAcquirdTowers()
    {
        foreach (var tower in acquirdTowers)
        {
            Destroy(tower.gameObject);
        }

        acquirdTowers.Clear();
    }

    public void SetAcquiredSelectedTower(LevelsMapAcquirdTower currentSelectedAcquiredTower, int ID)
    {
        this.currentSelectedAcquiredTower = currentSelectedAcquiredTower;

        if (TrySelectTowerByIndex(ID))
        {
            SetSelectedTowerUIParams(this.currentSelectedAcquiredTower, currentSelectedTowerUI.ID);
            towersDataHandler.SetSelectedTowersByIndex(selectedAcquiredTowersByIndex);

            currentSelectedTowerUI.UnSelect();
            currentSelectedTowerUI = null;

            HideAcquiredItemsContainer();
        }
    }

    private bool TrySelectTowerByIndex(int ID)
    {
        if (selectedAcquiredTowersByIndex[0] != ID && selectedAcquiredTowersByIndex[1] != ID && selectedAcquiredTowersByIndex[2] != ID && selectedAcquiredTowersByIndex[3] != ID)
        {
            selectedAcquiredTowersByIndex[currentSelectedTowerUI.ID] = ID;

            return true;
        }
        else
        {
            HideAcquiredItemsContainer();

            return false;
        }
    }

    private void SetSelectedTowerUIParams(LevelsMapAcquirdTower currentSelectedAcquiredTower, int index)
    {
        this.currentSelectedAcquiredTower = currentSelectedAcquiredTower;

        selectedTowerUIs[index].SetParams(currentSelectedAcquiredTower.TowerSO);
    }

    #endregion

    #region Heroes

    public void SetCurrentSelectedHeroUI(SelectedHeroUI selectedHeroUI)
    {
        CleanAllAcquiredItems();

        ShowAcquiredItemsContainer();

        CreateAcquiredHeroes();

        if (currentSelectedHeroUI != null && currentSelectedHeroUI != selectedHeroUI)
        {
            currentSelectedHeroUI.UnSelect();
        }

        currentSelectedHeroUI = selectedHeroUI;
    }

    private void CreateAcquiredHeroes()
    {
        if (currentSelectedTowerUI != null)
        {
            currentSelectedTowerUI.UnSelect();
            currentSelectedTowerUI = null;
        }

        for (int i = 0; allHeroes.AcquiredHeroes.Count > i; i++)
        {
            acquiredHeroes.Add(Instantiate(acquirdHeroPrefab, contentParentTransform, true));
            acquiredHeroes[i].SetHeroesDataHandler(this);
            acquiredHeroes[i].SetParams(allHeroes.AcquiredHeroes[i], i);
        }

        for (int i = 0; selectedHeroUIs.Count > i; i++)
        {
            selectedHeroUIs[i].Construct(this);
            selectedHeroUIs[i].SetParams(acquiredHeroes[selectedAcquiredHeroesByIndex[i]].HeroSO);
        }
    }

    private void CleanAcquirdHeros()
    {
        foreach (var hero in acquiredHeroes)
        {
            Destroy(hero.gameObject);
        }

        acquiredHeroes.Clear();
    }

    public void SetAcquiredSelectedHero(LevelsMapAcquirdHero currentSelectedAcquiredHero, int ID)
    {
        this.currentSelectedAcquiredHero = currentSelectedAcquiredHero;

        if (TrySelectHeroByIndex(ID))
        {
            SetSelectedHeroUIParams(currentSelectedAcquiredHero, currentSelectedHeroUI.ID);
            heroesDataHandler.SetSelectedHeroesByIndex(selectedAcquiredHeroesByIndex);
            HideAcquiredItemsContainer();
        }
    }

    private bool TrySelectHeroByIndex(int ID)
    {
        if (selectedAcquiredHeroesByIndex[0] != ID && selectedAcquiredHeroesByIndex[1] != ID)
        {
            selectedAcquiredHeroesByIndex[currentSelectedHeroUI.ID] = ID;

            return true;
        }
        else
        {
            HideAcquiredItemsContainer();

            return false;
        }
    }

    private void SetSelectedHeroUIParams(LevelsMapAcquirdHero currentSelectedAcquiredHero, int index)
    {
        this.currentSelectedAcquiredHero = currentSelectedAcquiredHero;

        selectedHeroUIs[index].SetParams(currentSelectedAcquiredHero.HeroSO);
    }

    #endregion
}
