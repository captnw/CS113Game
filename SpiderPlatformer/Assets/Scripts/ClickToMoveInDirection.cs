using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMoveInDirection : MonoBehaviour
{
    // basic click to jump script

    [SerializeField]
    private float m_power = 300f;

    private Vector3 m_force = Vector3.zero;

    private bool m_canMove = true;

    private Rigidbody2D m_rb;

    private StickToSurface m_sts;
    private CurrentlyTouching m_ct;

    private float m_degrees = 0;

    private Vector2 m_screenCenter;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_sts = GetComponent<StickToSurface>();
        m_ct = GetComponent<CurrentlyTouching>();

        m_ct.CollideStatusChangeEvent += CheckMoveCooldown;

        m_screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_canMove && Input.GetMouseButtonDown(0))
        {
            Vector3 worldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 mouseScreen = new Vector3(Input.mousePosition.x - Screen.width * 0.5f, Input.mousePosition.y - Screen.height * 0.5f, Input.mousePosition.z);

            if (CheckMousePositionValid(mouseScreen))
            {
                m_canMove = false;
                m_sts.Unstick();
                m_ct.ClearTouchedStuff();

                m_force = worldSpace - transform.position;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_force != Vector3.zero)
        {
            m_rb.AddForce(m_force * m_power, ForceMode2D.Force);
            m_force = Vector3.zero;
        }
    }

    private void CheckMoveCooldown()
    {
        // check if we're collided with something, if we did
        // then move cooldown is over

        if (m_ct.IsColliderTouchingAnything)
        {
            m_canMove = true;
        }
    }

    private bool CheckMousePositionValid(Vector3 adjustedMousePosition)
    {
        // prevent jumping into the ground, ground depends on whatever we're touching

        // pos y axis, 0 (pos x) -> pi (neg x)
        // neg y axis, 0 (pos x) -> -pi (neg x)
        float radians = Mathf.Atan2(adjustedMousePosition.y, adjustedMousePosition.x);

        float tolerance = Mathf.Deg2Rad * 180f; // the radius in which we cannot jump

        string msg = "";

        CurrentlyTouching.DirectionsCollided dirBitShift = m_ct.GetCollidingFacingDirections;

        bool collidedTop = dirBitShift.HasFlag(CurrentlyTouching.DirectionsCollided.TOP);
        bool collidedLeft = dirBitShift.HasFlag(CurrentlyTouching.DirectionsCollided.LEFT);
        bool collidedBottom = dirBitShift.HasFlag(CurrentlyTouching.DirectionsCollided.BOTTOM);
        bool collidedRight = dirBitShift.HasFlag(CurrentlyTouching.DirectionsCollided.RIGHT);

        if (collidedTop)
        {
            if (radians > 0 && radians > (Mathf.PI / 2 - tolerance / 2) && radians < (Mathf.PI / 2 + tolerance / 2))
            {
                Debug.Log("can't jump into top!!: " + radians);
                return false;
            }
        }
        else if (collidedBottom)
        {
            if (radians < 0 && Mathf.Abs(radians) > (Mathf.PI / 2 - tolerance) && Mathf.Abs(radians) < (Mathf.PI / 2 + tolerance))
            {
                Debug.Log("can't jump into bottom!!: " + radians);
                return false;
            }
        }
        else if (collidedLeft)
        {

        }

        Debug.Log("TOP: " + collidedTop + ", LEFT: " + collidedLeft + ", BOTTOM: " + collidedBottom + ", RIGHT: " + collidedRight);

        return true;
    }
}
