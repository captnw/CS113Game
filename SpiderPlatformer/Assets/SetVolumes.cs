using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolumes : MonoBehaviour
{
    // class is heavily inspired from this video: https://www.youtube.com/watch?v=xNHSGMKtlv4

    [SerializeField]
    private AudioMixer m_mixer;

    [SerializeField]
    private Slider m_masterSlider;

    [SerializeField]
    private Slider m_BGMSlider;

    [SerializeField]
    private Slider m_SFXSlider;

    private void Start()
    {
        // Set all the sliders to their appropriate values depending on the mixer
        float masterVol, BGMVol, SFXVol;

        if (m_mixer.GetFloat("MasterVol", out masterVol))
        {
            m_masterSlider.value = Mathf.Pow(10, masterVol / 20);
        }
        if (m_mixer.GetFloat("BGMVol", out BGMVol))
        {
            m_BGMSlider.value = Mathf.Pow(10, BGMVol / 20);
        }
        if (m_mixer.GetFloat("SFXVol", out SFXVol))
        {
            m_SFXSlider.value = Mathf.Pow(10, SFXVol / 20);
        }
    }

    /// <summary>
    /// Set volume of master.
    /// </summary>
    /// <param name="sliderValue">Volume (0.001->1) inclusive</param>
    public void SetMasterLevel(float sliderValue)
    {
        m_mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    /// <summary>
    /// Set volume of BGM.
    /// </summary>
    /// <param name="sliderValue">Volume (0.001->1) inclusive</param>
    public void SetBGMLevel(float sliderValue)
    {
        m_mixer.SetFloat("BGMVol", Mathf.Log10(sliderValue) * 20);
    }

    /// <summary>
    /// Set volume of BGM.
    /// </summary>
    /// <param name="sliderValue">Volume (0.001->1) inclusive</param>
    public void SetSFXLevel(float sliderValue)
    {
        m_mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }
}
