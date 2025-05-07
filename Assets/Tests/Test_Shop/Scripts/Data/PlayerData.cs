using Newtonsoft.Json;
using System;
using System.Collections.Generic;


public class PlayerData 
{
    private CharacterSkins selectedCharacterSkin;
    private MazeSkins selectedMazeSkin;

    private List<CharacterSkins> openCharacterSkins;
    private List<MazeSkins> openMazeSkins;

    private int money;

    public PlayerData()
    {
        money = 10000;

        selectedCharacterSkin = CharacterSkins.Buul;
        selectedMazeSkin = MazeSkins.Antarakuys;

        openCharacterSkins = new List<CharacterSkins>() { selectedCharacterSkin };
        openMazeSkins = new List<MazeSkins>() { selectedMazeSkin };
    }

    [JsonConstructor]
    public PlayerData(int money, CharacterSkins selectedCharacterSkin, MazeSkins selectedMazeSkin,
        List<CharacterSkins> openCharacterSkins, List<MazeSkins> openMazeSkins)
    {
        Money = money;

        this.selectedCharacterSkin = selectedCharacterSkin;
        this.selectedMazeSkin = selectedMazeSkin;

        this.openCharacterSkins = new List<CharacterSkins>(openCharacterSkins);
        this.openMazeSkins = new List<MazeSkins>(openMazeSkins);
    }

    public int Money
    {
        get => money;

        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            money = value;
        }
    }

    public CharacterSkins SelectedCharacterSkin
    {
        get => selectedCharacterSkin;

        set
        {
            if (openCharacterSkins.Contains(value) == false)
            {
                throw new ArgumentException(nameof(value));
            }

            selectedCharacterSkin = value;
        }
    }

    public MazeSkins SelectedMazeSkin
    {
        get => selectedMazeSkin;

        set
        {
            if (openMazeSkins.Contains(value) == false)
            {
                throw new ArgumentException(nameof(value));
            }

            selectedMazeSkin = value;
        }
    }

    public IEnumerable<CharacterSkins> OpenCharacterSkins => openCharacterSkins;
    public IEnumerable<MazeSkins> OpenMazeSkins => openMazeSkins;

    public void OpenCharacterSkin(CharacterSkins skin)
    {
        if (openCharacterSkins.Contains(skin))
        {
            throw new ArgumentException(nameof(skin));
        }

        openCharacterSkins.Add(skin);
    }

    public void OpenMazeSkin(MazeSkins skin)
    {
        if (openMazeSkins.Contains(skin))
        {
            throw new ArgumentException(nameof(skin));
        }

        openMazeSkins.Add(skin);
    }
}
