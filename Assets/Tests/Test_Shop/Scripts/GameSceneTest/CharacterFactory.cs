using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersFactory", menuName = "GamePlayExample/CharactersFactory")]
public class CharacterFactory : ScriptableObject
{
    [SerializeField] private CharacterMark character_Abo;
    [SerializeField] private CharacterMark character_Vxo;
    [SerializeField] private CharacterMark character_Dzvo;
    [SerializeField] private CharacterMark character_Buul;
    [SerializeField] private CharacterMark character_Tanjiro;

    public CharacterMark Get(CharacterSkins skinType, Vector3 spawnPosition)
    {
        CharacterMark instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, null);
        instance.Initialize();
        return instance;
    }

    private CharacterMark GetPrefab(CharacterSkins skinType)
    {
        switch (skinType)
        {
            case CharacterSkins.Abo:
                return character_Abo;

            case CharacterSkins.Vxo:
                return character_Vxo;
 
            case CharacterSkins.Dzvo:
                return character_Dzvo;

            case CharacterSkins.Buul:
                return character_Buul;

            case CharacterSkins.Tanjiro:
                return character_Tanjiro;

            default:
                throw new ArgumentException(nameof(skinType));
                
        }
    }
}
