using UnityEngine;

public class RotateThisObject : MonoBehaviour
{
    // This code is dependent on StickToSurface, we need a reference to it
    public delegate void Rotate(Quaternion rotation, bool hasLanded);

    [SerializeField]
    private StickToSurface m_ssScript;

    [SerializeField]
    private ClickToJumpTowardsDirection m_ctjScript;

    [SerializeField]
    private CurrentlyTouching m_ctScript;

    private SpriteRenderer m_sr;

    // Start is called before the first frame update
    void Start()
    {
        m_ssScript.ObjectsShouldRotate += RotateSelf;
        m_ctjScript.ObjectsShouldRotate += RotateSelf;

        m_sr = GetComponent<SpriteRenderer>();
    }

    private void RotateSelf(Quaternion rotation, bool hasLanded)
    {
        // hasLanded, check what direction we landed in so we may flip sprite 
        // flipX, if true, then we flipX, this is only true if we jump to the left

        m_sr.flipY = false;
        m_sr.flipX = false;

        gameObject.transform.rotation = rotation;

        if (rotation.eulerAngles.z >= 90 && rotation.eulerAngles.z <= 270)
        {
            m_sr.flipY = true;
        }

        if (hasLanded)
        {
            CurrentlyTouching.DirectionsCollided dirs = m_ctScript.GetCollidingFacingDirections;
            if (dirs.HasFlag(CurrentlyTouching.DirectionsCollided.TOP))
            {
                m_sr.flipY = true;
            }
        }

        //Debug.Log("rotation degrees: " + rotation.eulerAngles.z);
    }
}
