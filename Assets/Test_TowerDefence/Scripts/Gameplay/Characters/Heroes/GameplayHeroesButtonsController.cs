using System.Collections.Generic;
using UnityEngine;


public class GameplayHeroesButtonsController : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerInputHandler playerInputHandler;

    private List<BaseHero> heroes;
    private List<GameplayHeroButton> heroButtons;
    private List<GameplayHeroActiveSkillUI> heroActiveSkillsButtons;

    private GameplayHeroButton currentSelectedHeroButton;
    private GameplayHeroActiveSkillUI currentSelectedHeroActiveSkill;
    private LayerMask groundLayerMask;


    [Inject]
    public void Construct(PlayerInputHandler playerInputHandler)
    {
        this.playerInputHandler = playerInputHandler;
        playerInputHandler.TouchPressed += SelectHeroOnClick;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    public void SetCurrentSelectedHeroButton(GameplayHeroButton gameplayHeroButton)
    {
        if (currentSelectedHeroActiveSkill != null)
        {
            DeselectHeroesActiveSkillsButtons();
        }

        if (currentSelectedHeroButton != null && currentSelectedHeroButton != gameplayHeroButton)
        {
            currentSelectedHeroButton.SetSelected(false);
            heroes[currentSelectedHeroButton.ID].Deselect();

            currentSelectedHeroButton = gameplayHeroButton;

            currentSelectedHeroButton.SetSelected(true);
            heroes[currentSelectedHeroButton.ID].Select();
        }
        else if (currentSelectedHeroButton != null && currentSelectedHeroButton == gameplayHeroButton)
        {
            currentSelectedHeroButton.SetSelected(false);
            heroes[currentSelectedHeroButton.ID].Deselect();
            currentSelectedHeroButton = null;
        }
        else
        {
            currentSelectedHeroButton = gameplayHeroButton;
            currentSelectedHeroButton.SetSelected(true);
            heroes[currentSelectedHeroButton.ID].Select();
        }
    }

    public void SetCurrentSelectedHeroActiveSkillButton(GameplayHeroActiveSkillUI heroActiveSkillUI)
    {
        if (currentSelectedHeroButton)
        {
            DeselectHeroesControllButtons();
        }

        if(currentSelectedHeroActiveSkill != null && currentSelectedHeroActiveSkill != heroActiveSkillUI)
        {
            currentSelectedHeroActiveSkill.Deselect();
            currentSelectedHeroActiveSkill = heroActiveSkillUI;
        }
        else
        {
            currentSelectedHeroActiveSkill = heroActiveSkillUI;
        }

        heroes[currentSelectedHeroActiveSkill.ID].SuperAttack();
    }

    public void SetHeroesButtons(HeroesButtonsSpawner heroesButtonsSpawner)
    {
        heroButtons = heroesButtonsSpawner.GetHeroButtons();
        heroActiveSkillsButtons = heroesButtonsSpawner.GetHeroActiveSkillsButtons();
    }

    public void SetHeroes(List<BaseHero> heroes)
    {
        this.heroes = heroes;
    }

    public void DeselectHeroesControllButtons()
    {
        for (int i = 0; i < heroButtons.Count; i++)
        {
            heroButtons[i].SetSelected(false);
            heroes[i].Deselect();
        }

        currentSelectedHeroButton = null;
    }

    public void DeselectHeroesActiveSkillsButtons()
    {
        for (int i = 0; i < heroActiveSkillsButtons.Count; i++)
        {
            heroActiveSkillsButtons[i].Deselect();
        }
        currentSelectedHeroActiveSkill = null;
    }

    public void SelectHeroOnClick(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 150, groundLayerMask))
        {
            DeselectHeroesControllButtons();
        }
    }

    private void OnDisable()
    {
        playerInputHandler.TouchPressed -= SelectHeroOnClick;
    }
}
