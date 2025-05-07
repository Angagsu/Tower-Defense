using System;
using UnityEngine.UI;
using UnityEngine;

public class ShopCategoryButton : MonoBehaviour
{
    public event Action Click;

    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Color selectColor;
    [SerializeField] private Color unselectColor;

    
    public void Select() => image.color = selectColor;
    public void Unselect() => image.color = unselectColor;
    private void OnClick() => Click?.Invoke();

    private void OnEnable() => button.onClick.AddListener(OnClick);
    private void OnDisable() => button.onClick.RemoveListener(OnClick);
}
