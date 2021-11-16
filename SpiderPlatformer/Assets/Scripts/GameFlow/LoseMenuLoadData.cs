using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseMenuLoadData : MonoBehaviour
{
    [SerializeField]
    private DataTransition m_dataObj;

    [SerializeField]
    private TextMeshProUGUI m_timerText;

    // Start is called before the first frame update
    void Start()
    {
        m_timerText.text = m_dataObj.minutes.ToString("00") + ":" + m_dataObj.seconds.ToString("00.00");
    }
}
