using System.Linq;

public class OpenSkinsChecker : IShopItemVisitor
{
    private IPersistentData persistentData;

    public bool IsOpened { get; private set; }

    public OpenSkinsChecker(IPersistentData persistentData)
        => this.persistentData = persistentData;

    public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);

    public void Visit(CharacterSkinItem characterSkinItem) => IsOpened =
        persistentData.PlayerData.OpenCharacterSkins.Contains(characterSkinItem.SkinType);

    public void Visit(MazeSkinItem mazeSkinItem) => IsOpened =
        persistentData.PlayerData.OpenMazeSkins.Contains(mazeSkinItem.SkinType);
}
