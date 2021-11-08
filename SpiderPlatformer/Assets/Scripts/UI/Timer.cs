using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // just count up forever and store the time and display the time ok!

    // we don't store hours 
    private int minutes = 0;
    private float seconds = 0;
    private string text;
    [SerializeField]
    private TextMeshProUGUI timerText;

    // Update is called once per frame
    void Update()
    {
        if (seconds >= 60)
        {
            minutes++;
            seconds %= 60;
        }
        seconds += Time.deltaTime;

        UpdateText();
    }

    // Update the timer text
    private void UpdateText()
    {
        text = minutes.ToString("00") + ":" + seconds.ToString("00.00");
        timerText.text = text;
        GameManager.Instance.setText(text);
    }

    private float GetSeconds()
    {
        return seconds;
    }

    private int GetMinutes()
    {
        return minutes;
    }
}
