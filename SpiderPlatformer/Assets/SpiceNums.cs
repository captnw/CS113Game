using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpiceNums : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI spiceCounterText;
    // Start is called before the first frame update
    void Start()
    {
        spiceCounterText.text = "" + GameManager.Instance.returnSpiceNum();

    }
}
