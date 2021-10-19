using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThisObject : MonoBehaviour
{
    // This code is dependent on StickToSurface, we need a reference to it
    public delegate void EventCallback(Quaternion rotation);

    [SerializeField]
    private StickToSurface m_ssScript;

    [SerializeField]
    private ClickToJumpTowardsDirection m_ctjScript;

    // Start is called before the first frame update
    void Start()
    {
        m_ssScript.ObjectsShouldRotate += RotateSelf;
        m_ctjScript.ObjectsShouldRotate += RotateSelf;
    }

    private void RotateSelf(Quaternion newRot)
    {
        gameObject.transform.rotation = newRot;
    }
}
