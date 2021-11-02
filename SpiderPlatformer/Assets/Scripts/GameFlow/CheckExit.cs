using UnityEngine;

public class CheckExit : MonoBehaviour
{
    // if you collected all spices and you touch this collider, you win this level

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            GameManager gm = GameManager.Instance;

            if (gm.HasCollectedAllSpices())
            {
                Debug.Log("YOU WIN!");
            }
        }
    }
}
