using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThisObject : MonoBehaviour
{
    // This code is dependent on StickToSurface, we need a reference to it
    [SerializeField]
    private StickToSurface m_ssScript;

    // Start is called before the first frame update
    void Start()
    {
        m_ssScript.ObjectsShouldRotate += RotateSelf;
    }

    private void RotateSelf(Quaternion newRot)
    {
        gameObject.transform.rotation = newRot;
    }
}
