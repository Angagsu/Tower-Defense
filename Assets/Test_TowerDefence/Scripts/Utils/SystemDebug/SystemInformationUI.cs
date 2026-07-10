using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemInformationUI : MonoBehaviour
{
    [SerializeField] private SystemInformation systemInformation;
    [SerializeField] private Button sIButton;
    [SerializeField] private Button[] statsHeaderButtons;
    [SerializeField] private GameObject systemInformPanel;
    [SerializeField] private List<SystemInfoStatsUI> statsTexts;

    private SystemInfoStatsUI currentActiveStatsText;


    private void Awake()
    {
        sIButton.onClick.AddListener(ToggleSystemInformationPanel);

        for (int i = 0; i < statsHeaderButtons.Length; i++)
        {
            int index = i; // Capture the current value of i
            statsHeaderButtons[i].onClick.AddListener(() => ToggleStatsText(index));
        }
    }

    private void ToggleStatsText(int index)
    {
        if (currentActiveStatsText != null && currentActiveStatsText != statsTexts[index])
        {
            currentActiveStatsText.Hide();
            currentActiveStatsText = statsTexts[index];
            currentActiveStatsText.Show();

            for (int i = 0; i < statsTexts[index].StatsTexts.Length; i++)
            {
                statsTexts[index].StatsTexts[i].text = systemInformation.AllStats[index][i];
            }
        }
        else if (currentActiveStatsText && currentActiveStatsText == statsTexts[index])
        {
            currentActiveStatsText.Hide();
            currentActiveStatsText = null;
        }
        else
        {
            currentActiveStatsText = statsTexts[index];
            currentActiveStatsText.Show();

            for (int i = 0; i < statsTexts[index].StatsTexts.Length; i++)
            {
                statsTexts[index].StatsTexts[i].text = systemInformation.AllStats[index][i];
            }
        }
    }

    private void ToggleSystemInformationPanel()
    {
        systemInformPanel.SetActive(!systemInformPanel.activeSelf);
    }
}