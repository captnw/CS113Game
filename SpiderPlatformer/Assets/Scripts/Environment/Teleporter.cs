using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // This is a one way teleporter (simple)

    [SerializeField]
    private GameObject m_destinationDoor;

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            collision.gameObject.SetActive(false);
            collision.gameObject.transform.position = m_destinationDoor.transform.position + new Vector3(0, 0.5f, 0);
            collision.gameObject.SetActive(true);
        }
    }
}
