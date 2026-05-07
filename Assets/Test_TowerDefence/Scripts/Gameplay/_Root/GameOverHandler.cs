using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameOverHandler : MonoBehaviour 
{
    public event Action<int[], List<TowerCard>, List<HeroCard>> ExtraLootReceived;

    [SerializeField] private GameplayMainUI gameplayMainUI;
    [SerializeField] private GameplayPlayerDataHandler gameplayPlayerDataHandler;
    [SerializeField] private GameplayStates gameplayStates;
    

    [SerializeField] private ReachedStarsUI reachedStarsUI;

    private AsyncSceneLoadService sceneLoadService;
    private LevelsMapDataStoreService levelsDataStoreService;
    private GamePlayerDataHandler playerDataHandler;
    private HeroesDataStoreService heroesDataStoreService;
    private TowersDataStoreService towersDataStoreService;
    private GameplayAchievements gameplayAchievements;
    private GameplayExtraLoot extraLoot;


    [Inject]
    public void Construct(AsyncSceneLoadService sceneLoadService, LevelsMapDataStoreService levelsDataStoreService, GamePlayerDataHandler playerDataHandler, HeroesDataStoreService heroesDataStoreService, TowersDataStoreService towersDataStoreService)
    {
        this.sceneLoadService = sceneLoadService;
        this.levelsDataStoreService = levelsDataStoreService;
        this.playerDataHandler = playerDataHandler;
        this.heroesDataStoreService = heroesDataStoreService;
        this.towersDataStoreService = towersDataStoreService;

        gameplayAchievements = new();
        extraLoot = new();

        gameplayAchievements.SetPlayerStartLives(gameplayPlayerDataHandler.Lives);
    }

    private void OnEnable()
    {
        gameplayPlayerDataHandler.LivesAmountChanged += OnPlayerLivesChange;
    }

    private void OnPlayerLivesChange(int lives)
    {
        if (lives <= 0)
        {
            Defeat();
        }
    }


    private void OnDisable()
    {
        gameplayPlayerDataHandler.LivesAmountChanged -= OnPlayerLivesChange;
    }

    private void Defeat()
    {
        gameplayStates.SetGameplaySpeed(timeScale: 1);
        gameplayStates.SetStateDefeat();
        gameplayMainUI.DefeatPanel.SetActive(true);
    }

    [ContextMenu("LevelComplete")]
    public void LevelComplete()
    {
        gameplayStates.SetGameplaySpeed(timeScale: 1);

        //int reachedStarsAmount = gameplayAchievements.CalculateStars(gameplayPlayerDataHandler.Lives);
        int reachedStarsAmount = 3;
        var loot = extraLoot.CalculateExtraLoot(reachedStarsAmount);

        List<HeroCard>  lootedHeroCards = extraLoot.GetHeroCardLoot();
        List<TowerCard> lootedTowerCards = extraLoot.GetTowerCardLoot();

        reachedStarsUI.ShowStars(reachedStarsAmount);

        gameplayMainUI.LevelCompletePanel.SetActive(true);

        ExtraLootReceived?.Invoke(loot, lootedTowerCards, lootedHeroCards);

        gameplayStates.SetStateComplete();

       // gameplayMainUI.StartCoroutine(gameplayMainUI.AnimateWaveSurvivedText());

        playerDataHandler.AddEnergy(extraLoot.Energy);
        playerDataHandler.AddGold(extraLoot.Gold);
        if (lootedHeroCards != null) heroesDataStoreService.AddCard(lootedHeroCards);
        if (lootedTowerCards != null) towersDataStoreService.AddCard(lootedTowerCards);        

        levelsDataStoreService.SaveLevelAchievements(isLevelAchieved: true, reachedStarsAmount);
    }

    public void RetryButton()
    {
        gameplayStates.SetGameplaySpeed(timeScale: 1);
        sceneLoadService.LoadSceneByName(SceneManager.GetActiveScene().name);
        gameplayMainUI.PausePanel.SetActive(false);
    }

    public void MenuButton()
    {
        gameplayStates.SetGameplaySpeed(timeScale: 1);
        sceneLoadService.LoadSceneByName(Scenes.LEVELS_MAP);
        gameplayMainUI.PausePanel.SetActive(false);
    }

    

    public void PauseOrContinueTheGame()
    {

    }
}
