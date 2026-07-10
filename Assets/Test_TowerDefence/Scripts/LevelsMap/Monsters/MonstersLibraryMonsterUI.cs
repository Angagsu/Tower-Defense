using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonstersLibraryMonsterUI : MonoBehaviour
{
    public MonsterSO MonsterSO { get => monsterSO; }

    [SerializeField] private Button button;
    [SerializeField] private Image monsterImage;
    [SerializeField] private Image selectImage;


    private MonsterSO monsterSO;
    private MonstersLibraryController monsterLibraryController;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        monsterLibraryController.SetSelectedMonster(this);
    }

    public void SetParams(MonsterSO monsterSO, MonstersLibraryController monstersLibraryController)
    {
        this.monsterSO = monsterSO;
        this.monsterLibraryController = monstersLibraryController;

        monsterImage.sprite = monsterSO.Sprite;

        UnSelect();
    }

    public void SetSelectedMonster(CardsExchangeHeroCardUI selectedHeroCard, bool isSecondCardCheaper = false)
    {
        if (selectedHeroCard == null)
        {
            selectImage.gameObject.SetActive(false);
            return;
        }
 
        selectImage.gameObject.SetActive(true);
    }

    public void Select() => selectImage.gameObject.SetActive(true);

    public void UnSelect() => selectImage.gameObject.SetActive(false);
}
