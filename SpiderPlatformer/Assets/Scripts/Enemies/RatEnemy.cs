using SpiderPlatformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemy : MonoBehaviour, IKillPlayer, IMoving
{
    public float Speed { get { return m_speed; } set { m_speed = value; } }
    [SerializeField]
    private float m_speed = 30f;

    private const float m_fallMultiplier = 5f; // fall faster, make character feel less floaty

    private bool m_isMovingLeft = true;

    [SerializeField]
    private float m_raycastDistance = 3f;

    private const string PLAYER_TAG = "Player";

    private const string PLATFORM_LAYER_NAME = "Platform";

    private static LayerMask m_platformLayerIndex = 0;
    static int m_platformLayerMask;

    // only raycast every 0.5f second
    private const float RAYCAST_LIMIT = 0.5f;
    private float m_raycastTimer = 0f;

    private const float RAYCAST_ANGLE_DEGREES = 30f;

    // references
    private Rigidbody2D m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();

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

        Move();

        m_raycastTimer += Time.time;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(PLAYER_TAG))
        {
            Kill(collision.collider.gameObject);
        }
    }

    public void Kill(GameObject victim)
    {
        Destroy(victim);
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
