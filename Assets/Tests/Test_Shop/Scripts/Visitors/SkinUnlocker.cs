using UnityEngine;

public class SkinUnlocker : MonoBehaviour, IShopItemVisitor
{
    private IPersistentData persistentData;

    public SkinUnlocker(IPersistentData persistentData) =>
        this.persistentData = persistentData;

    public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);
    
    public void Visit(CharacterSkinItem characterSkinItem) =>
        persistentData.PlayerData.OpenCharacterSkin(characterSkinItem.SkinType);
    
    public void Visit(MazeSkinItem mazeSkinItem) =>
        persistentData.PlayerData.OpenMazeSkin(mazeSkinItem.SkinType);
    
}
