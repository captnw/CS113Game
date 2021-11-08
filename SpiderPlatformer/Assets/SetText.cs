using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(GameManager.Instance.returnTimer());
        GameManager.Instance.returnTimer();
    }


}
