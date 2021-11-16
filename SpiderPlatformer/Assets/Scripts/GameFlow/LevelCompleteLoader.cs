using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteLoader : MonoBehaviour
{
    [SerializeField]
    private DataTransition m_dataOBJ;

    [SerializeField]
    private TextMeshProUGUI m_timerText;

    [SerializeField]
    private TextMeshProUGUI m_spiceText;

    private System.Func<int, int, string> spiceFormatter = (int collected, int max) =>
    {
        return $"{collected} out of {max} spices collected";
    };

    // stars are currently semi-hardcoded, if you collect 1 spice, 1 star ; 2 spice, 2 stars , etc etc...
    [SerializeField]
    private GameObject m_star1;
    [SerializeField]
    private GameObject m_star2;
    [SerializeField]
    private GameObject m_star3;

    // Start is called before the first frame update
    void Start()
    {
        m_timerText.text = m_dataOBJ.minutes.ToString("00") + ":" + m_dataOBJ.seconds.ToString("00.00");
        m_spiceText.text = spiceFormatter(m_dataOBJ.numSpicesCollected, m_dataOBJ.numSpicesLevel);

        int collected = m_dataOBJ.numSpicesCollected;

        if (collected <= 2)
        {
            m_star1.SetActive(false);
        }
        if (collected <= 1)
        {
            m_star2.SetActive(false);
        }
        if (collected == 0)
        {
            m_star3.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
