using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCollider : MonoBehaviour
{
    // This code is dependent on CurrentlyTouching, we need a reference to it
    [SerializeField]
    private CurrentlyTouching m_ctScript;

    [SerializeField]
    private CurrentlyTouching.DirectionCollider m_direction; // the direction that this collider is facing, needs to be set in Unity

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG))
        {
            m_ctScript.DirectionColliderEvent.Invoke(m_direction);
        }
    }
}
