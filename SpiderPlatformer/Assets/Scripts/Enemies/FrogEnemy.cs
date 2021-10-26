using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEnemy : MonoBehaviour, IKillPlayer
{
    private GameObject m_player;

    [SerializeField]
    private float m_frogDetectionRadius = 10f;

    // references

    [SerializeField]
    private GameObject m_frogTongue;

    [SerializeField]
    private GameObject m_frogEye; // where the tongue will be extending from

    [SerializeField]
    private LineRenderer m_lr;

    private float m_frogAttackRadius = 8f; // the radius where the frog will attack

    private float m_timeForFrogToExtendTongue = 0.25f;

    private bool m_isTargetting = false;

    // Start is called before the first frame update
    void Start()
    {
        Intialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isTargetting)
        {
            float dist = Vector2.Distance(transform.position, m_player.transform.position);

            if (dist <= m_frogDetectionRadius)
            {

                if (dist <= m_frogAttackRadius)
                {
                    StartCoroutine(ShootTongue(m_player.transform.position));
                }

                Vector2 diff = m_player.transform.position - transform.position;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);

            }
            else
            {
                if (transform.eulerAngles != Vector3.zero)
                {
                    transform.eulerAngles = Vector3.zero;
                }
            }

        }
    }

    private IEnumerator ShootTongue(Vector2 target)
    {
        m_isTargetting = true;

        GameObject tongue = Instantiate(m_frogTongue, m_frogEye.transform);
        tongue.transform.rotation = transform.rotation;

        float m_startTime = Time.time;
        float m_endTime = m_startTime + m_timeForFrogToExtendTongue;

        // Extend tongue
        while (Time.time < m_endTime)
        {
            tongue.transform.position = Vector2.Lerp(m_frogEye.transform.position, target, (Time.time - m_startTime) / m_timeForFrogToExtendTongue);
            DrawTongue(m_frogEye.transform.position, tongue.transform.position);
            yield return null;
        }
        tongue.transform.position = target;
        DrawTongue(m_frogEye.transform.position, tongue.transform.position);

        // Unextend tongue
        m_startTime = Time.time;
        m_endTime = m_startTime + m_timeForFrogToExtendTongue;
        
        while (Time.time < m_endTime)
        {
            tongue.transform.position = Vector2.Lerp(target, m_frogEye.transform.position, (Time.time - m_startTime) / m_timeForFrogToExtendTongue);
            DrawTongue(m_frogEye.transform.position, tongue.transform.position);
            yield return null;
        }
        tongue.transform.position = m_frogEye.transform.position;
        DrawTongue(m_frogEye.transform.position, tongue.transform.position);

        Destroy(tongue);

        DrawTongue(Vector2.zero, Vector2.zero);

        m_isTargetting = false;
    }

    private void DrawTongue(Vector2 start, Vector2 end)
    {
        m_lr.SetPosition(0, start);
        m_lr.SetPosition(1, end);
    }

    private void Intialize()
    {
        string namePlayer = "CharacterCage";
        m_player = GameObject.Find(namePlayer);
    }

    public void Kill(GameObject victim)
    {
        Destroy(victim);
    }
}
