using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceCollect : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            GameManager.Instance.CollectedSpice();
            Destroy(gameObject);
        }
    }
}
