using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyTouching : MonoBehaviour
{
    // Works as long as you don't jump into the floor (cause sliding)

    // This is essentially a bitmask (useful for representing directions... because it could be top, bottom, or both of them at the same time)
    [Flags]
    public enum DirectionsCollided
    {
        NONE = 0,
        TOP = (1 << 0),
        LEFT = (1 << 1),
        BOTTOM = (1 << 2),
        RIGHT = (1 << 3)
    };

    // delegate that tells you whenever we are colliding or no longer colliding
    // CollideStatusChangeEvent is a publish / subscribe sort of thing
    public delegate void OnFirstCollideOrNoLongerColliding();
    public OnFirstCollideOrNoLongerColliding CollideStatusChangeEvent;

    // IsColliderTouchingAnything is a public accessor (see c# definition for public accessors)
    public bool IsColliderTouchingAnything
    {
        get { return m_isColliderTouchingAnything; }
        private set { m_isColliderTouchingAnything = value; }
    }
    private bool m_isColliderTouchingAnything = false;

    public DirectionsCollided GetCollidingFacingDirections
    {
        get { return m_DirCollidedBitShift; }
    }
    private DirectionsCollided m_DirCollidedBitShift = DirectionsCollided.NONE;

    private HashSet<GameObject> m_stuffTouching; // store the set of GameObjects that is touching the spider
    
    public GameObject FirstCollidedObject
    {
        get { return m_firstObject ?? null; }
        private set { m_firstObject = value; }
    }
    private GameObject m_firstObject;

    // Const variables, these won't be changed.
    private const string PLAYER_TAG = "Player";
    private const string PLATFORM_LAYER_NAME = "Platform";

    private LayerMask m_platformLayerIndex;
    int m_platformLayerMask;

    #region Monobehaviour functions

    // Start is called before the first frame update
    private void Start()
    {
        m_stuffTouching = new HashSet<GameObject>();
        m_platformLayerIndex = LayerMask.NameToLayer(PLATFORM_LAYER_NAME);
        m_platformLayerMask = (1 << m_platformLayerIndex);
    }

    #endregion

    #region Rigidbody2D callbacks

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Make sure the gameObject doesn't have a player tag, and its layer belongs to the platforms layers (we only want platforms)
        if (!collision.gameObject.CompareTag(PLAYER_TAG) && collision.gameObject.layer == m_platformLayerIndex)
        {
            // The null check is necessary for invoking the CollideStatusChange delegate, while IsNotTouchingAnything ensures 
            // this block of code only runs once
            if (CollideStatusChangeEvent != null && !IsColliderTouchingAnything)
            {
                GetDirectionColliding();

                m_firstObject = collision.collider.gameObject;
                IsColliderTouchingAnything = true;
                CollideStatusChangeEvent(); // invoke delegate to tell subscribers that we're now colliding

                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("JumpLand");
                }
            }

            m_stuffTouching.Add(collision.collider.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG) && collision.gameObject.layer == m_platformLayerIndex)
        {
            m_stuffTouching.Remove(collision.collider.gameObject);

            if (CollideStatusChangeEvent != null && IsColliderTouchingAnything)
            {
                IsColliderTouchingAnything = false;
                CollideStatusChangeEvent();
                m_firstObject = null;
            }
        }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Clear the m_DirCollidedBitShift bits (set it to zero)
    /// </summary>
    private void ClearDirectionsColliding()
    {
        m_DirCollidedBitShift = DirectionsCollided.NONE;
    }

    /// <summary>
    /// This should be called only once if we collided with a platform for the first time.
    /// We should a little ray (a line originating from a point) in the cardinal directions (up, left, down, right)
    /// to see what surfaces we're connected to. 
    /// </summary>
    private void GetDirectionColliding()
    {
        const float RAYCAST_DISTANCE = 1.25f;

        // TOP,LEFT,BOTTOM,RIGHT
        Vector2[] directions = { Vector2.up, Vector2.left, Vector2.down, Vector2.right };

        for (int dirInd = 0; dirInd < directions.Length; dirInd++)
        {
            RaycastHit2D raycastInfo = Physics2D.Raycast(transform.position, directions[dirInd], RAYCAST_DISTANCE, m_platformLayerMask);

            Debug.DrawRay(transform.position, directions[dirInd] * RAYCAST_DISTANCE, Color.yellow, 10f); // uncomment and check in scene view for cool visual effects

            // If the raycastInfo.collider is valid, then we have collided with something!
            if (raycastInfo.collider != null)
            {
                //Debug.Log(directions[dirInd] + " " + raycastInfo.collider.name);

                //m_directionsColliding[dirInd] = true;

                switch (dirInd)
                {
                    case 0:
                        // top
                        m_DirCollidedBitShift |= DirectionsCollided.TOP; // OR the bitshift to add the infomation
                        break;
                    case 1:
                        // left
                        m_DirCollidedBitShift |= DirectionsCollided.LEFT;
                        break;
                    case 2:
                        // bottom
                        m_DirCollidedBitShift |= DirectionsCollided.BOTTOM;
                        break;
                    case 3:
                        // right
                        m_DirCollidedBitShift |= DirectionsCollided.RIGHT;
                        break;
                }

            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Used by the ClickToMoveInDirection script to clear the stuff we've touched and the directions we collided in.
    /// Essentially, these need to be resetted everytime we do a jump.
    /// </summary>
    public void ClearTouchedStuff()
    {
        m_stuffTouching.Clear();
        ClearDirectionsColliding();
    }

    #endregion
}
