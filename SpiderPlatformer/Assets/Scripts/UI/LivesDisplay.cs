using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    // attach this script to the lives container

    // This class is also a singleton (only one LivesDisplay!)
    private static LivesDisplay _instance;
    public static LivesDisplay Instance { get { return _instance; } }

    private int m_numLives;

    private Sprite m_fullHeart;
    private Sprite m_emptyHeart;

    private const string ICON_PATH = "Sprites/Icons/";
    private const string FULL_HEART = "HeartFull";
    private const string EMPTY_HEART = "HeartEmpty";

    private Image[] m_lives;

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

    // Start is called before the first frame update
    void Start()
    {
        m_fullHeart = Resources.Load<Sprite>(ICON_PATH + FULL_HEART);
        m_emptyHeart = Resources.Load<Sprite>(ICON_PATH + EMPTY_HEART);

        m_numLives = gameObject.transform.childCount;

        m_lives = new Image[m_numLives];

        for (int i = 0; i < m_numLives; i++)
        {
            m_lives[i] = gameObject.transform.GetChild(i).GetComponent<Image>();
        }
    }

    /// <summary>
    /// Player has died, set the sprite of the previous number of lives-th heart to empty
    /// </summary>
    public void PlayerDied()
    {
        m_lives[GameManager.Instance.PlayerLives - 1].sprite = m_emptyHeart;
    }
}
