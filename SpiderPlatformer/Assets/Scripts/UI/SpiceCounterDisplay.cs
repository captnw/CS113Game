using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpiceCounterDisplay : MonoBehaviour
{
    // attach this script to the spice counter displayer

    // This class is also a singleton (only one spice display!)
    private static SpiceCounterDisplay _instance;
    public static SpiceCounterDisplay Instance { get { return _instance; } }

    [SerializeField]
    private TextMeshProUGUI spiceCounterText;

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

    /// <summary>
    /// Update the spice counter display.
    /// </summary>
    /// <param name="numCollected">Number of spices you collected.</param>
    /// <param name="numNeededToCollect">Number of spices you need to collect.</param>
    public void UpdateText(int numCollected, int numNeededToCollect)
    {
        spiceCounterText.text = numCollected + " of " + numNeededToCollect;
    }

}
