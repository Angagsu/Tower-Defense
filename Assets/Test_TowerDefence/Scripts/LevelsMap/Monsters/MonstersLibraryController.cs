using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonstersLibraryController : MonoBehaviour
{
    [SerializeField] private Image monsterImage;
    [SerializeField] private MonstersLibraryMonsterUI monsterPerfabUI;
    [SerializeField] private RectTransform monstersContainer;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI uniqueSkillNameText;
    [SerializeField] private TextMeshProUGUI uniqueSkillDescriptionText;
    [SerializeField] private TextMeshProUGUI monsterTypeText;
    [Space(10)]
    [SerializeField] private MonstersLibraryStatsHandlerUI monstersStatsHandler;
    [SerializeField] private List<MonstersLibraryMonstersByTypeUI> monstersGroupByType;


    private List<MonstersLibraryMonsterUI> monstersUI = new List<MonstersLibraryMonsterUI>();

    private MonstersLibraryMonstersByTypeUI selectedMonstersGroupByType;
    private MonstersLibraryMonsterUI selectedMonsterUI;


    private void Start()
    {
        selectedMonstersGroupByType = monstersGroupByType[0];

        SetSelectedMonstersGroupByType(selectedMonstersGroupByType);
    }

    public void SetSelectedMonstersGroupByType(MonstersLibraryMonstersByTypeUI selectedMonstersGroupByType)
    {
        if(this.selectedMonstersGroupByType != null)
        {
            this.selectedMonstersGroupByType.UnSelect();
        }

        this.selectedMonstersGroupByType = selectedMonstersGroupByType;
        this.selectedMonstersGroupByType.Select();

        ClearMonsters();

        CreateMonstersByType(this.selectedMonstersGroupByType.Monsters);

        SetSelectedMonster(monstersUI[0]);
    }

    public void SetSelectedMonster(MonstersLibraryMonsterUI selectedMonsterUI)
    {
        if(this.selectedMonsterUI != null)
        {
            this.selectedMonsterUI.UnSelect();
        }

        this.selectedMonsterUI = selectedMonsterUI;
        this.selectedMonsterUI.Select();


        monstersStatsHandler.UpdateStats(this.selectedMonsterUI.MonsterSO);
        
        SetupText(this.selectedMonsterUI.MonsterSO);
    }

    private void SetupText(MonsterSO monsterSO)
    {
        monsterTypeText.text = $"{monsterSO.Type}";
        uniqueSkillNameText.text = $"Unique Skill:   {monsterSO.UniqueSkillType}";
        uniqueSkillDescriptionText.text = monsterSO.UniqueSkillDescription;
        

        monsterImage.sprite = monsterSO.Sprite;
    }

    private void CreateMonstersByType(MonstersByTypeSO monstersByTypeSO)
    {
        for (int i = 0; i < monstersByTypeSO.Monsters.Count; i++)
        {
            var monsterView = Instantiate(monsterPerfabUI, monstersContainer);
            monsterView.SetParams(monstersByTypeSO.Monsters[i], this);
            monstersUI.Add(monsterView);
        }
    }

    private void ClearMonsters()
    {
        if (monstersUI.Count == 0)
            return;

        for (int i = 0; i < monstersUI.Count; i++)
        {
            Destroy(monstersUI[i].gameObject);
        }

        monstersUI.Clear();
    }
}
