using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSurface : MonoBehaviour
{
    // current bug, if you jump into the ground while on a slope, the Stick()
    // function will not be invoked (because you never left the ground)
    // a.k.a cannot jump no more
    // sliding is not nice to you

    public delegate void EventCallback(Quaternion rotation);
    public EventCallback ObjectsShouldRotate;

    private CurrentlyTouching m_ct;
    private Rigidbody2D m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();

        m_ct = GetComponent<CurrentlyTouching>();
        m_ct.CollideStatusChangeEvent += Stick;
    }

    private void Stick()
    {
        // free position and rotations (from physics)
        if (m_ct.IsColliderTouchingAnything)
        {
            //print(m_ct.FirstCollidedObject.name);

            if (ObjectsShouldRotate != null)
            {
                ObjectsShouldRotate.Invoke(m_ct.FirstCollidedObject.transform.rotation);
            }

            m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void Unstick()
    {
        // free rotations (from physics)
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
