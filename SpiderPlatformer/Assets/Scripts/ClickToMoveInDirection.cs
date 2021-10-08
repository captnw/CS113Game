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

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_sts = GetComponent<StickToSurface>();
        m_ct = GetComponent<CurrentlyTouching>();

        m_ct.CollideStatusChangeEvent += CheckMoveCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        // you can move in the air, haven't implemented the check for this yet
        if (m_canMove && Input.GetMouseButtonDown(0))
        {
            m_canMove = false;
            m_sts.Unstick();
            m_ct.ClearTouchedStuff();

            Vector3 worldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //print("Test: " + worldSpace.x + " " + worldSpace.y);

            m_force = worldSpace - transform.position;
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
}
