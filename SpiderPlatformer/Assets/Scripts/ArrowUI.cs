using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUI : MonoBehaviour
{
    Rigidbody2D rb;
    Transform a_transform;
    private float m_power = 300f;
    public GameObject player1;
    private Vector3 m_force = Vector3.zero;
    public GameObject point;
    public GameObject tip;
    GameObject[] points;
    public int numOfPts;
    public float spaceBetweenPts;
    Vector2 direction;



    private void Start()
    {
        rb = player1.GetComponent<Rigidbody2D>();
        a_transform = GetComponent<Transform>();
        points = new GameObject[numOfPts];
        for (int i = 0; i < numOfPts; i++)
        {
            if(i != numOfPts - 1)
            {
                points[i] = Instantiate(point, a_transform.position, Quaternion.identity);

            }
            else
            {
                points[i] = Instantiate(tip, a_transform.position, Quaternion.identity);
            }
        }
    }
    void Update()
    {
        Vector2 aPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - aPosition;
        transform.right = direction;
        a_transform.position = player1.transform.position;
        print(aPosition);
        print(player1.transform.position);
        m_force = mousePosition - rb.position;
        //if (input.getmousebuttondown(1))
        //{
        //    jamesjump();
        //}

        for(int i = 0; i < numOfPts; i++)
        {
            points[i].transform.position = PointPosition(i * spaceBetweenPts);
        }
    }

    //void jamesjump()
    //{
    //    rb.addforce(m_force * m_power, forcemode2d.force);
    //    m_force = vector3.zero;
    //}

    Vector2 PointPosition(float t)
    {
        Vector2 position = (Vector2)a_transform.position + (direction.normalized * t);
        return position;
    }
}
