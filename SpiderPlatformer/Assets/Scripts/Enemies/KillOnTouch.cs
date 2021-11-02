using UnityEngine;

public class KillOnTouch : MonoBehaviour, IKillPlayer
{
    private const string PLAYER_TAG = "Player";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(PLAYER_TAG))
        {
            Kill(collision.collider.gameObject);
        }
    }

    public void Kill(GameObject victim)
    {
        victim.SetActive(false);
        GameManager.Instance.CheckRespawn();
    }
}
