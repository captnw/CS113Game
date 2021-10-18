using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyTouching : MonoBehaviour
{
    // Works as long as you don't jump into the floor (cause sliding)

    [Flags]
    public enum DirectionsCollided
    {
        NONE = 0,
        TOP = (1 << 0),
        LEFT = (1 << 1),
        BOTTOM = (1 << 2),
        RIGHT = (1 << 3)
    };

    public delegate void OnFirstCollideOrNoLongerColliding();
    public OnFirstCollideOrNoLongerColliding CollideStatusChangeEvent; // delegate that tells you whenever we are colliding or no longer colliding

    public bool IsColliderTouchingAnything
    {
        get { return m_isColliderTouchingAnything; }
        private set { m_isColliderTouchingAnything = value; }
    }
    private bool m_isColliderTouchingAnything = false;

    public DirectionsCollided GetCollidingFacingDirections
    {
        get 
        {
            return m_DirCollidatedBitShift;
        }
    }
    private DirectionsCollided m_DirCollidatedBitShift = DirectionsCollided.NONE;

    private HashSet<GameObject> m_stuffTouching; // store the set of GameObjects that is touching the spider
    
    public GameObject FirstCollidedObject
    {
        get { return m_firstObject ?? null; }
        private set { m_firstObject = value; }
    }
    private GameObject m_firstObject;

    private const string PLAYER_TAG = "Player";
    private const string PLATFORM_LAYER_NAME = "Platform";

    private LayerMask m_platformLayerIndex;

    #region Monobehaviour functions

    // Start is called before the first frame update
    private void Start()
    {
        m_stuffTouching = new HashSet<GameObject>();
        m_platformLayerIndex = LayerMask.NameToLayer(PLATFORM_LAYER_NAME);
    }

    #endregion

    #region Rigidbody2D callbacks

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG) && collision.gameObject.layer == m_platformLayerIndex)
        {
            if (CollideStatusChangeEvent != null && IsNotTouchingAnything())
            {
                GetDirectionColliding();

                m_firstObject = collision.collider.gameObject;
                IsColliderTouchingAnything = true;
                CollideStatusChangeEvent();
            }

            m_stuffTouching.Add(collision.collider.gameObject);
        }
    }

    // don't use oncollisionstay2d b/c you will be stopped above the ground
    // when you jump, and you got to click again to jump

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG) && collision.gameObject.layer == m_platformLayerIndex)
        {
            m_stuffTouching.Remove(collision.collider.gameObject);

            if (CollideStatusChangeEvent != null && IsNotTouchingAnything())
            {
                IsColliderTouchingAnything = false;
                CollideStatusChangeEvent();
                m_firstObject = null;
            }
        }
    }

    #endregion

    #region Private methods

    private void ClearDirectionsColliding()
    {
        m_DirCollidatedBitShift = DirectionsCollided.NONE;
    }

    /// <summary>
    /// This should be called only once if we collided with a platform for the first time.
    /// </summary>
    private void GetDirectionColliding()
    {
        const float RAYCAST_DISTANCE = 1f;

        // TOP,LEFT,BOTTOM,RIGHT
        Vector2[] directions = { Vector2.up, Vector2.left, Vector2.down, Vector2.right };

        int layerMask = (1 << m_platformLayerIndex); // bit masking, the 1 bit is the layer we want to focus on

        for (int dirInd = 0; dirInd < directions.Length; dirInd++)
        {
            RaycastHit2D raycastInfo = Physics2D.Raycast(transform.position, directions[dirInd], RAYCAST_DISTANCE, layerMask);

            //Debug.DrawRay(transform.position, directions[dirInd] * RAYCAST_DISTANCE, Color.yellow, 10f);

            if (raycastInfo.collider != null)
            {
                //Debug.Log(directions[dirInd] + " " + raycastInfo.collider.name);

                //m_directionsColliding[dirInd] = true;

                switch (dirInd)
                {
                    case 0:
                        // top
                        m_DirCollidatedBitShift |= DirectionsCollided.TOP;
                        break;
                    case 1:
                        // left
                        m_DirCollidatedBitShift |= DirectionsCollided.LEFT;
                        break;
                    case 2:
                        // bottom
                        m_DirCollidatedBitShift |= DirectionsCollided.BOTTOM;
                        break;
                    case 3:
                        // right
                        m_DirCollidatedBitShift |= DirectionsCollided.RIGHT;
                        break;
                }

            }
        }
    }

    private bool IsNotTouchingAnything()
    {
        return m_stuffTouching.Count == 0;
    }

    #endregion

    #region Public methods

    public void ClearTouchedStuff()
    {
        m_stuffTouching.Clear();
        ClearDirectionsColliding();
    }

    #endregion
}
