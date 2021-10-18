using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowThis : MonoBehaviour
{
    // Attach this to the gameObject the camera should follow
    // no lerping so far

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }
}
