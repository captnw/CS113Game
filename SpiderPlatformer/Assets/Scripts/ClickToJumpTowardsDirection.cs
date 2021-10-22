using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToJumpTowardsDirection : MonoBehaviour
{
    // basic click to jump script
    // Harder to jump into the floor, also corners are no good

    [SerializeField]
    private float m_power = 300f; // The power of the jump

    [SerializeField]
    private float m_fallMultiplier = 2.5f; // fall faster, make character feel less floaty

    private Vector3 m_direction = Vector3.zero; // stores the direction that you're jumping in

    private bool m_canMove = true; // Stores the boolean that is used to check and prevent jumping in mid air

    // references
    private Rigidbody2D m_rb = null;
    private StickToSurface m_sts = null;
    private CurrentlyTouching m_ct = null;

    private const string PLATFORM_LAYER_NAME = "Platform";

    private LayerMask m_platformLayerIndex;
    int m_platformLayerMask;

    //public delegate void EventCallback(Vector2 mousePos);
    //public EventCallback ObjectShouldFlip; // this delegate is used to flip the sprite once we jump

    public RotateThisObject.EventCallback ObjectsShouldRotate; // this delegate is used to rotate the sprite once it jumps

    private void Start()
    {
        // fetch references
        m_rb = GetComponent<Rigidbody2D>();
        m_sts = GetComponent<StickToSurface>();
        m_ct = GetComponent<CurrentlyTouching>();

        m_ct.CollideStatusChangeEvent += CheckMoveCooldown; // subscribe to the CollideStatusChangeEvent, we need to know when this happens

        m_platformLayerIndex = LayerMask.NameToLayer(PLATFORM_LAYER_NAME);
        m_platformLayerMask = (1 << m_platformLayerIndex);
    }

    // Update is called once per frame
    private void Update()
    {
        // If we can move, and we left click...
        if (m_canMove && Input.GetMouseButtonDown(0))
        {
            Vector3 mouseInputScreenSpace = Input.mousePosition;

            // convert mousePosition from screenSpace, to worldSpace
            // because our gameObject is in worldSpace, and if we subtract
            // the two positions, we can fetch the direction to the mousePosition
            Vector3 worldSpace = Camera.main.ScreenToWorldPoint(mouseInputScreenSpace);

            // And we're not jumping into the floor, then allow the character to jump in this direction
            if (IsMousePositionValid(worldSpace))
            {
                m_canMove = false; // prevent jumping mid air
                m_sts.Unstick(); // unstick the object, we can now move
                m_ct.ClearTouchedStuff(); // this needs to be called everytime we jump

                if (ObjectsShouldRotate != null)
                {
                    Vector2 test = new Vector2(mouseInputScreenSpace.x - Screen.width * 0.5f, mouseInputScreenSpace.y - Screen.height * 0.5f);
                    Quaternion newQ = Quaternion.Euler(0, 0, Mathf.Atan2(test.y, test.x) * Mathf.Rad2Deg);

                    ObjectsShouldRotate.Invoke(newQ); // flip the character to face the mouse when jumping
                }

                m_direction = (worldSpace - transform.position).normalized; // normalize the direction so we can ignore distance (the vector's magnitude is simply 1)
                m_rb.velocity = m_direction * m_power; // set the velocity to move the character
            }
        }

        // If we're falling, fall faster over time
        // Code snippet is from: https://www.youtube.com/watch?v=7KiK0Aqtmzc
        if (m_rb.velocity.y < 0)
        {
            m_rb.velocity += Vector2.up * Physics2D.gravity.y * (m_fallMultiplier - 1) * Time.deltaTime;
        }
    }

    /// <summary>
    /// This function will be subscribed to m_ct.CollideStatusChangeEvent, so whenever that is invoked, this is called.
    /// </summary>
    private void CheckMoveCooldown()
    {
        // check if we're collided with something, if we did
        // then move cooldown is over
        if (m_ct.IsColliderTouchingAnything)
        {
            m_canMove = true;
        }
    }

    /// <summary>
    /// Prevents jumping into the ground, ground depends on whatever we're touching.
    /// Trigonometry is involved.
    /// </summary>
    /// <param name="mousePositionWorldSpace">Pass in the mousePosition after it is converted to world space.</param>
    /// <returns>True if we're not jumping into a platform, false otherwise.</returns>
    private bool IsMousePositionValid(Vector2 mousePositionWorldSpace)
    {
        // Raycast to see where we cannot jump
        const float RAYCAST_DISTANCE = 2f;

        Vector2 dir = (mousePositionWorldSpace - (Vector2)transform.position).normalized; // calculate direction

        RaycastHit2D raycastInfo = Physics2D.Raycast(transform.position, dir, RAYCAST_DISTANCE, m_platformLayerMask); // shoot a ray

        //Debug.DrawRay(transform.position, dir * RAYCAST_DISTANCE, Color.red, 10f); // uncomment and check in scene view for cool visual effects

        // If the raycastInfo.collider is valid, then we have collided with something!
        if (raycastInfo.collider != null && (!m_ct.FirstCollidedObject || raycastInfo.collider.gameObject == m_ct.FirstCollidedObject))
        {
            // raycastInfo.collider.gameObject == m_ct.FirstCollidedObject) is required so that
            // we know we've collided with the same object we had collided with earlier, so this jump
            // is invalid
            return false;
        }

        return true;
    }
}
