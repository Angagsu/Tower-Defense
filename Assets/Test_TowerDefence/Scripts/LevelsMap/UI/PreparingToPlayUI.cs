using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreparingToPlayUI : MonoBehaviour
{
    [Header("Acquired Heroes")]
    [Space(5)]
    [SerializeField] private LevelsMapAcquirdHero acquirdHeroPrefab;
    [SerializeField] private GameObject acquiredHeroesGameObject;
    [SerializeField] private Transform contentParentTransform;
    [Space(10)]
    [SerializeField] private List<SelectedHeroUI> selectedHeroUIs;
    [SerializeField] private Button levelStartButton;
    [SerializeField] private TextMeshProUGUI energyCostText;
    [Space(10)]
    [SerializeField] private LevelsMapDataHandler levelsMapDataHandler;
    [SerializeField] private HeroesDataHandler heroesDataHandler;
    [SerializeField] private List<Button> quitButtons;
    [SerializeField] private GameObject preparingToPlayPanel;

    private List<LevelsMapAcquirdHero> acquiredHeroes = new();   
    private List<int> selectedAcquiredHeroesByIndex;

    private LevelsMapAcquirdHero currentSelectedAcquiredHero;
    private SelectedHeroUI currentSelectedHeroUI;


    private void Awake()
    {
        selectedAcquiredHeroesByIndex = heroesDataHandler.SelectedHeroesByIndex;

        for(int i = 0; quitButtons.Count > i; i++)
        {
            quitButtons[i].onClick.AddListener(OnQuit);
        }

        levelStartButton.onClick.AddListener(OnLevelStartButtonClick);
        energyCostText.text = levelsMapDataHandler.PlayCost.ToString();
    }

    private void OnEnable()
    {
        var heroes = heroesDataHandler.GetLevelsMapHeroes();

        for (int i = 0; heroes.Count > i; i++)
        {
            if (heroes[i].HeroData.IsAcquired)
            {
                acquiredHeroes.Add(Instantiate(acquirdHeroPrefab, contentParentTransform, true));
                acquiredHeroes[i].SetHeroesDataHandler(this);
                acquiredHeroes[i].SetParams(heroes[i].HeroSO, i);
            }
        }

        for (int i = 0; selectedHeroUIs.Count > i; i++)
        {
            selectedHeroUIs[i].Construct(this);
            selectedHeroUIs[i].SetParams(acquiredHeroes[selectedAcquiredHeroesByIndex[i]].HeroSO);
        }
    }

    public void OnLevelStartButtonClick()
    {
        if (levelsMapDataHandler.TryToStartPlay())
        {
            heroesDataHandler.SaveSelectedHeroesBeforeStartPlayLevel();
        }
    }

    private void SetSelectedHeroUIParams(LevelsMapAcquirdHero currentSelectedAcquiredHero, int index)
    {
        this.currentSelectedAcquiredHero = currentSelectedAcquiredHero;

        selectedHeroUIs[index].SetParams(currentSelectedAcquiredHero.HeroSO);
    }

    public void SetAcquiredSelectedHero(LevelsMapAcquirdHero currentSelectedAcquiredHero, int ID)
    {
        this.currentSelectedAcquiredHero = currentSelectedAcquiredHero;

        if (TrySelectHeroByIndex(ID))
        {
            SetSelectedHeroUIParams(currentSelectedAcquiredHero, currentSelectedHeroUI.ID);
            heroesDataHandler.SetSelectedHeroesByIndex(selectedAcquiredHeroesByIndex);
        }
    }

    public void SetCurrentSelectedHeroUI(SelectedHeroUI selectedHeroUI)
    {
        this.currentSelectedHeroUI = selectedHeroUI;

        ShowAcquirdHeroesObj();
    }

    public void HideAcquirdHeroesObj() => acquiredHeroesGameObject.SetActive(false);

    public void ShowAcquirdHeroesObj() => acquiredHeroesGameObject.SetActive(true);

    private bool TrySelectHeroByIndex(int ID)
    {
        if (selectedAcquiredHeroesByIndex[0] != ID && selectedAcquiredHeroesByIndex[1] != ID)
        {
            selectedAcquiredHeroesByIndex[currentSelectedHeroUI.ID] = ID;
            currentSelectedAcquiredHero = null;
            HideAcquirdHeroesObj();
            return true;
        }
        else
        {
            currentSelectedAcquiredHero = null;
            currentSelectedHeroUI.UnSelect();
            HideAcquirdHeroesObj();
            return false;
        }
    }

    private void ClearAcquirdHeros()
    {
        foreach (var hero in acquiredHeroes)
        {
            Destroy(hero.gameObject);
        }

        acquiredHeroes.Clear();
    }

    private void OnQuit()
    {
        ClearAcquirdHeros();
        HideAcquirdHeroesObj();
        preparingToPlayPanel.SetActive(false);
    }
}
