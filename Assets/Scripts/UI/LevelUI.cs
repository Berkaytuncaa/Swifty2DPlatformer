using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Button[] buttons;
    public TextMeshProUGUI[] chapterTimes;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;

            // Retrieve saved time, default to "0:00" if none
            string savedTime = PlayerPrefs.GetString("ChapterTime_" + i, "0:00");
            chapterTimes[i].text = savedTime;

            Color textColor = chapterTimes[i].color;
            textColor.a = 0.5f;
            chapterTimes[i].color = textColor;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;

            Color textColor = chapterTimes[i].color;
            textColor.a = 1f;
            chapterTimes[i].color = textColor;
        }
    }
}
