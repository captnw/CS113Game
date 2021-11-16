using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // just count up forever and store the time and display the time ok!

    private static Timer _instance;
    public static Timer Instance { get { return _instance; } }

    // we don't store hours 
    public int Minutes { get { return minutes; } }
    public float Seconds { get { return seconds; } }

    private int minutes = 0;
    private float seconds = 0;
    private string text;
    [SerializeField]
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        // Ensure that this class a singleton (you can invoke this within any script as long as GameManager exists on a GameObject anywhere in the scene)
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

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
