using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyTouching : MonoBehaviour
{
    // Works as long as you don't jump into the floor (cause sliding)

    public delegate void OnFirstCollideOrNoLongerColliding();
    public OnFirstCollideOrNoLongerColliding CollideStatusChangeEvent;

    public bool IsColliderTouchingAnything
    {
        get { return m_isColliderTouchingAnything; }
        private set { m_isColliderTouchingAnything = value; }
    }
    private bool m_isColliderTouchingAnything = false;

    private HashSet<GameObject> m_stuffTouching; // store the set of GameObjects that is touching the spider
    
    public GameObject FirstCollidedObject
    {
        get { return m_firstObject ?? null; }
        private set { m_firstObject = value; }
    }
    private GameObject m_firstObject;

    #region Monobehaviour functions

    // Start is called before the first frame update
    private void Start()
    {
        m_stuffTouching = new HashSet<GameObject>();
    }

    #endregion

    #region Rigidbody2D callbacks

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CollideStatusChangeEvent != null && IsStuffTouchingEmpty())
        {
            m_firstObject = collision.collider.gameObject;
            IsColliderTouchingAnything = true;
            CollideStatusChangeEvent();
        }

        m_stuffTouching.Add(collision.collider.gameObject);
    }

    // don't use oncollisionstay2d b/c you will be stopped above the ground
    // when you jump, and you got to click again to jump

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_stuffTouching.Remove(collision.collider.gameObject);

        if (CollideStatusChangeEvent != null && IsStuffTouchingEmpty())
        {
            IsColliderTouchingAnything = false;
            CollideStatusChangeEvent();
            m_firstObject = null;
        }
    }

    #endregion

    #region Private methods

    private bool IsStuffTouchingEmpty()
    {
        return m_stuffTouching.Count == 0;
    }

    #endregion

    #region Public methods

    public void ClearTouchedStuff()
    {
        m_stuffTouching.Clear();
    }

    #endregion
}
