using System;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ShopItemView : MonoBehaviour, IPointerClickHandler
{
    public event Action<ShopItemView> Click;

    [SerializeField] private Sprite standartBackground;
    [SerializeField] private Sprite highlightBackground;

    [SerializeField] private Image contentImage;
    [SerializeField] private Image lockImage;

    [SerializeField] private Image selectionText;

    [SerializeField] private IntValueView priceView;

    private Image backgroundImage;

    public ShopItem Item { get; private set; }
    public bool IsLock { get; private set; }
    public int Price => Item.Price;
    public GameObject Model => Item.Model;

    public void Intitialize(ShopItem item)
    {
        backgroundImage = GetComponent<Image>();
        backgroundImage.sprite = standartBackground;

        Item = item;
        contentImage.sprite = item.Image;

        priceView.Show(Price);
    }
    public void OnPointerClick(PointerEventData eventData) => Click?.Invoke(this);

    public void Lock()
    {
        IsLock = true;
        lockImage.gameObject.SetActive(IsLock);
        priceView.Show(Price);
    }

    public void UnLock()
    {
        IsLock = false;
        lockImage.gameObject.SetActive(IsLock);
        priceView.Hide();
    }

    public void Select() => selectionText.gameObject.SetActive(true);
    public void UnSelect() => selectionText.gameObject.SetActive(false);

    public void Highlight() => backgroundImage.sprite = highlightBackground;
    public void UnHighlight() => backgroundImage.sprite = standartBackground;
}
