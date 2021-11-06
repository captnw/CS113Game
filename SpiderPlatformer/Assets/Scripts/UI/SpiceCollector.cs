using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpiceCollector : MonoBehaviour
{
    private float spice = 0;
    public TextMeshProUGUI spiceCount;
    public TextMeshProUGUI EndSpiceCount;
    private float health = 3;
    public GameObject h1;
    public GameObject h2;
    public GameObject h3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Spice")
        {
            spice++;
            spiceCount.text = spice.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "EndGame")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
        if (collision.transform.tag == "Enemy")
        {
            health--;
            print("Health " + health);
            if(health == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
            }
        }
    }
}
