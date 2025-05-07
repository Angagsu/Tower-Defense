public class SelectedSkinChecker : IShopItemVisitor
{
    private IPersistentData persistentData;

    public bool IsSelected { get; private set; }

    public SelectedSkinChecker(IPersistentData persistentData)
        => this.persistentData = persistentData;

    public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);

    public void Visit(CharacterSkinItem characterSkinItem) => IsSelected =
        persistentData.PlayerData.SelectedCharacterSkin == characterSkinItem.SkinType;

    public void Visit(MazeSkinItem mazeSkinItem) => IsSelected =
        persistentData.PlayerData.SelectedMazeSkin == mazeSkinItem.SkinType;
}
