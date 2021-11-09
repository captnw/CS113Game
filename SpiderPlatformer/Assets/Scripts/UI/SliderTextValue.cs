using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValue : MonoBehaviour
{
    // given a slider and a TextMeshPro text object, set the text object's text property to whatever the slider's value is at the moment (and set it whenever the
    // slider changes)

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Slider sliderRef;

    public void SetValue()
    {
        timerText.text = Mathf.FloorToInt(sliderRef.value / sliderRef.maxValue * 100).ToString();
    }
}
