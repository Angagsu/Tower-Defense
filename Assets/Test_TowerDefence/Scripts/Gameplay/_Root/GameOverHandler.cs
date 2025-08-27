using UnityEngine;
using UnityEngine.SceneManagement;



public class GameOverHandler : MonoBehaviour 
{
    [SerializeField] private GameplayMainUI gameplayMainUI;
    [SerializeField] private GameplayPlayerDataHandler gameplayPlayerDataHandler;
    [SerializeField] private GameplayStates gameplayStates;
    

    [SerializeField] private ReachedStarsUI reachedStarsUI;

    private AsyncSceneLoadService sceneLoadService;
    private LevelsMapDataHandler levelsDataHandler;
    private GameplayAchievements gameplayAchievements;


    [Inject]
    public void Construct(AsyncSceneLoadService sceneLoadService, LevelsMapDataHandler levelsDataHandler)
    {
        this.sceneLoadService = sceneLoadService;
        this.levelsDataHandler = levelsDataHandler;

        gameplayAchievements = new();
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
        gameplayMainUI.GameOverPanel.SetActive(true);
    }

    [ContextMenu("LevelComplete")]
    public void LevelComplete()
    {
        gameplayStates.SetGameplaySpeed(timeScale: 1);

        int reachedStarsAmount = gameplayAchievements.CalculateStars(gameplayPlayerDataHandler.Lives);

        reachedStarsUI.ShowStars(reachedStarsAmount);

        gameplayMainUI.LevelCompletePanel.SetActive(true);

        gameplayStates.SetStateComplete();

        gameplayMainUI.StartCoroutine(gameplayMainUI.AnimateWaveSurvivedText());

        //levelsDataHandler.SaveLevelAchievements(true, reachedStarsAmount);
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
