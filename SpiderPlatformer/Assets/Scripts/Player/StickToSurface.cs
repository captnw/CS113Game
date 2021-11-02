using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSurface : MonoBehaviour
{
    // This script actually handles "sticking". However, it is dependent
    // on the delegate CollideStatusChangeEvent from the CurrentlyTouching class (basically an event).

    public RotateThisObject.Rotate ObjectsShouldRotate; // this delegate is used to rotate the sprite if its on a slope

    private CurrentlyTouching m_ct;
    private Rigidbody2D m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();

        m_ct = GetComponent<CurrentlyTouching>();
        m_ct.CollideStatusChangeEvent += Stick;
    }

    /// <summary>
    /// Prevents the gameObject this script is attached to from moving if it has a rigidbody.
    /// </summary>
    private void Stick()
    {
        if (m_rb == null)
        {
            Debug.LogError("you're going to have to attach a Rigidbody2D to this gameObject.");
            return;
        }

        // free position and rotations (from physics)
        if (m_ct.IsColliderTouchingAnything)
        {
            //print(m_ct.FirstCollidedObject.name);

            if (ObjectsShouldRotate != null)
            {
                ObjectsShouldRotate.Invoke(m_ct.FirstCollidedObject.transform.rotation, true);
            }

            m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    /// <summary>
    /// Allows the gameObject this script is attached to move if it has a rigidbody.
    /// </summary>
    public void Unstick()
    {
        if (m_rb == null)
        {
            Debug.LogError("you're going to have to attach a Rigidbody2D to this gameObject.");
            return;
        }

        // free rotations (from physics)
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
