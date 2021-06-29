using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform thistp;
    Transform mainCam;
    private void Start()
    {
        thistp = transform;
    }
    private void LateUpdate()
    {
        if (mainCam == null)
        {
            mainCam = UIManager.Instance.GetGameCam().transform;
        }
        thistp.LookAt(mainCam.position);
        thistp.Rotate(0, 180, 0);
    }
}
