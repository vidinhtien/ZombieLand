using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public int order = 0;
    private void Start()
    {

    }
    void Update()
    {
        //Chup man o day
        if (Input.GetKeyDown(KeyCode.C))
        {
            ScreenCapture.CaptureScreenshot("ScreenShot_" + order);
            order++;
            Debug.Log("Captured " + order);
        }
        // Next Level o day
        if (Input.GetKeyDown(KeyCode.N))
        {

        }
    }
}
