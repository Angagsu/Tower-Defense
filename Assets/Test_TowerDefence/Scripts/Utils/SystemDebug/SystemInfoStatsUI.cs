using TMPro;
using UnityEngine;

public class SystemInfoStatsUI : MonoBehaviour
{
    public TextMeshProUGUI[] StatsTexts;

    public bool IsSelected => gameObject.activeSelf;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
