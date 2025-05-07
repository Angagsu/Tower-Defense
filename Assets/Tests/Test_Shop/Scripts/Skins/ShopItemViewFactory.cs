using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemViewFactory", menuName = "Shop/ShopItemViewFactory")]
public class ShopItemViewFactory : ScriptableObject
{
    [SerializeField] private ShopItemView characterSkinItemPrefab;
    [SerializeField] private ShopItemView mazeSkinItemPrefab;

    public ShopItemView Get(ShopItem shopItem, Transform parent)
    {
        ShopItemVisitor visitor = new ShopItemVisitor(characterSkinItemPrefab, mazeSkinItemPrefab);
        visitor.Visit(shopItem);

        ShopItemView instance = Instantiate(visitor.Prefab, parent);
        instance.Intitialize(shopItem);

        return instance;
    }

    private class ShopItemVisitor : IShopItemVisitor
    {
        private ShopItemView characterSkinItemPrefab;
        private ShopItemView mazeSkinItemPrefab;

        public ShopItemVisitor(ShopItemView characterSkinItemPrefab, ShopItemView mazeSkinItemPrefab)
        {
            this.characterSkinItemPrefab = characterSkinItemPrefab;
            this.mazeSkinItemPrefab = mazeSkinItemPrefab;
        }
        public ShopItemView Prefab { get; private set; }

        public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);

        public void Visit(CharacterSkinItem characterSkinItem) => Prefab = characterSkinItemPrefab;

        public void Visit(MazeSkinItem mazeSkinItem) => Prefab = mazeSkinItemPrefab;
    }

}
