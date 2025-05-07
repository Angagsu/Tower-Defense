using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopContent", menuName = "Shop/SopContent")]
public class ShopContent : ScriptableObject
{
    [SerializeField] private List<CharacterSkinItem> characterSkinItems; 
    [SerializeField] private List<MazeSkinItem> mazeSkinItems;

    public IEnumerable<CharacterSkinItem> CharacterSkinItems => characterSkinItems;
    public IEnumerable<MazeSkinItem> MazeSkinItems => mazeSkinItems;

    private void OnValidate()
    {
        var characterSkinsDuplicates = characterSkinItems.GroupBy(item => item.SkinType)
            .Where(array => array.Count() > 1);

        if (characterSkinsDuplicates.Count() > 0)
        {
            throw new InvalidOperationException(nameof(characterSkinItems));
        }

        var mazeSkinsDuplicates = mazeSkinItems.GroupBy(item => item.SkinType)
            .Where(array => array.Count() > 1);

        if (mazeSkinsDuplicates.Count() > 0)
        {
            throw new InvalidOperationException(nameof(mazeSkinItems));
        }
    }
}
