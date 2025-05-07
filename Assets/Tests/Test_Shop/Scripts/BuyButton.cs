using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public event Action Click;

    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;

    [SerializeField] private Color lockColor;
    [SerializeField] private Color unlockColor;

    [SerializeField, Range(0, 1)] private float lockAnimationDuration = 0.4f;
    [SerializeField, Range(0.5f, 5)] private float lockAnimationStrength = 2f;

    private bool isLock;

    private void OnEnable() => button.onClick.AddListener(OnButtonClick);
    private void OnDisable() => button.onClick.RemoveListener(OnButtonClick);

    public void UpdateText(int price) => text.text = price.ToString();

    public void Lock()
    {
        isLock = true;
        text.color = lockColor;
    }

    public void Unlock()
    {
        isLock = false;
        text.color = unlockColor;
    }
    private void OnButtonClick()
    {
        if (isLock)
        {
            transform.DOShakePosition(lockAnimationDuration, lockAnimationStrength);
            return;
        }
        Click?.Invoke();
    }
}
