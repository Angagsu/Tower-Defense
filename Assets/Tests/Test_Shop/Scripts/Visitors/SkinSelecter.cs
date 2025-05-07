public class SkinSelecter : IShopItemVisitor
{
    private IPersistentData persistentData;

    public SkinSelecter(IPersistentData persistentData)
        => this.persistentData = persistentData;

    public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);

    public void Visit(CharacterSkinItem characterSkinItem) => 
        persistentData.PlayerData.SelectedCharacterSkin = characterSkinItem.SkinType;

    public void Visit(MazeSkinItem mazeSkinItem) => 
        persistentData.PlayerData.SelectedMazeSkin = mazeSkinItem.SkinType;
}
