using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public event Action<ShopItemView> ItemViewClicked;

    private List<ShopItemView> shopItems = new List<ShopItemView>();

    [SerializeField] private Transform itemsParent;
    [SerializeField] private ShopItemViewFactory shopItemViewFactory;

    private OpenSkinsChecker openSkinsChecker;
    private SelectedSkinChecker selectedSkinChecker;

    public void Initialize(OpenSkinsChecker openSkinsChecker, SelectedSkinChecker selectedSkinChecker)
    {
        this.openSkinsChecker = openSkinsChecker;
        this.selectedSkinChecker = selectedSkinChecker;
    }

    public void Show(IEnumerable<ShopItem> items)
    {
        Clear();

        foreach (ShopItem item in items)
        {
            ShopItemView spawnedItem = shopItemViewFactory.Get(item, itemsParent);
            spawnedItem.Click += OnItemViewClick;

            spawnedItem.UnSelect();
            spawnedItem.UnHighlight();

            openSkinsChecker.Visit(spawnedItem.Item);

            if (openSkinsChecker.IsOpened)
            {
                selectedSkinChecker.Visit(spawnedItem.Item);

                if (selectedSkinChecker.IsSelected)
                {
                    spawnedItem.Select();
                    spawnedItem.Highlight();
                    ItemViewClicked?.Invoke(spawnedItem);
                }

                spawnedItem.UnLock();
            }
            else
            {
                spawnedItem.Lock();
            }

            shopItems.Add(spawnedItem);
        }

        Sort();
    }

    public void Select(ShopItemView itemView)
    {
        foreach (var item in shopItems)
        {
            item.UnSelect();
        }

        itemView.Select();
    }

    private void Sort()
    {
        shopItems = shopItems
            .OrderBy(item => item.IsLock)
            .ThenByDescending(item => item.Price)
            .ToList();

        for (int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].transform.SetSiblingIndex(i);
        }
    }

    private void OnItemViewClick(ShopItemView itemView)
    {
        Highlight(itemView);
        ItemViewClicked?.Invoke(itemView);
    }

    private void Highlight(ShopItemView shopItemView)
    {
        foreach (var item in shopItems)
        {
            item.UnHighlight();
        }

        shopItemView.Highlight();
    }

    private void Clear()
    {
        foreach (ShopItemView item in shopItems)
        {
            item.Click -= OnItemViewClick;
            Destroy(item.gameObject);
        }
        shopItems.Clear();
    }
}
