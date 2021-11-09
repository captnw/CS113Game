using SpiderPlatformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemy : MonoBehaviour, IMoving
{
    public float Speed { get { return m_speed; } set { m_speed = value; } }
    [SerializeField]
    private float m_speed = 30f;

    private const float m_fallMultiplier = 5f; // fall faster, make character feel less floaty

    private bool m_isMovingLeft = true;

    [SerializeField]
    private float m_raycastDistance = 3f;

    private const string PLATFORM_LAYER_NAME = "Platform";

    private static LayerMask m_platformLayerIndex = 0;
    static int m_platformLayerMask;

    // only raycast every 0.5f second
    private const float RAYCAST_LIMIT = 0.5f;
    private float m_raycastTimer = 0f;

    private const float RAYCAST_ANGLE_DEGREES = 30f;

    // references
    [SerializeField]
    private Rigidbody2D m_rb;

    [SerializeField]
    private SpriteRenderer m_sr;

    private const float MIN_SQEAK_TIME = 3f;
    private const float MAX_SQEAK_TIME = 7f;
    private float m_timeUntilSqeak = 3f;

    private void Start()
    {
        if (m_platformLayerMask == 0)
        {
            m_platformLayerIndex = LayerMask.NameToLayer(PLATFORM_LAYER_NAME);
            m_platformLayerMask = (1 << m_platformLayerIndex);
        }

        m_isMovingLeft = Random.Range(0, 1) > 0.5 ? m_isMovingLeft : !m_isMovingLeft; // randomize moving left or right
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("IsMovingLeft: " + m_isMovingLeft);

        m_timeUntilSqeak -= Time.deltaTime;

        if (m_timeUntilSqeak <= 0)
        {
            m_timeUntilSqeak = Random.Range(MIN_SQEAK_TIME, MAX_SQEAK_TIME);

            if (AudioManager.instance)
            {
                // Rat must be on screen to play SFX
                Vector3 viewPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

                if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
                {
                    AudioManager.instance.Play("MouseSqueak");
                }
            }
        }

        Move();

        m_raycastTimer += Time.time;

        m_sr.flipX = m_isMovingLeft;
    }

    private void FixedUpdate()
    {
        // limit raycasts, they're expensive
        if (m_raycastTimer > RAYCAST_LIMIT)
        {
            m_raycastTimer = 0;

            RaycastHit2D hitWall = Physics2D.Raycast(transform.position, m_isMovingLeft ?
                Utility.rotate(Vector2.left, -RAYCAST_ANGLE_DEGREES * Mathf.Deg2Rad) : Utility.rotate(Vector2.right, RAYCAST_ANGLE_DEGREES * Mathf.Deg2Rad), m_raycastDistance, m_platformLayerMask);

            //Debug.DrawRay(transform.position, m_isMovingLeft ? Utility.rotate(Vector2.left, -RAYCAST_ANGLE_DEGREES * Mathf.Deg2Rad) * m_raycastDistance : Utility.rotate(Vector2.right, RAYCAST_ANGLE_DEGREES * Mathf.Deg2Rad) * m_raycastDistance, Color.green, 5);

            if (hitWall)
            {
                //Debug.Log("hit wall!");
                m_isMovingLeft = !m_isMovingLeft;
            }
        }
    }

    public void Move()
    {
        transform.Translate(m_isMovingLeft ? Vector2.left * m_speed * Time.deltaTime : Vector2.right * m_speed * Time.deltaTime);

        // If we're falling, fall faster over time
        Vector2 temp2 = m_rb.velocity;
        Utility.fallfaster(ref temp2, m_fallMultiplier);
        m_rb.velocity = temp2;
    }
}
