using UnityEngine;
using UnityEngine.UI;

public class MonstersLibraryMonstersByTypeUI : MonoBehaviour
{
    [field: SerializeField] public MonstersByTypeSO Monsters { get; private set; }

    [SerializeField] private Image monstersTypeImage;
    [SerializeField] private Button button;
    [SerializeField] private Image selectImage;
    [SerializeField] private MonstersLibraryController monstersLibraryController;

    private int id;


    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetCardsController(CardsExchangeControllerUI cardsController)
    {
        //this.cardsController = cardsController;
    }

    public void SetParams(MonstersByTypeSO monstersByType, int iD)
    {
        this.Monsters = monstersByType;
        this.id = iD;

        //monstersTypeImage.sprite = monstersByType.Sprite;

        UnSelect();
    }

    private void OnClick()
    {
        monstersLibraryController.SetSelectedMonstersGroupByType(this);
    }

    public void Select() => selectImage.gameObject.SetActive(true);
    public void UnSelect() => selectImage.gameObject.SetActive(false);
}
