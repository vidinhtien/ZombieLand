using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwieRotate : MonoBehaviour
{

    Transform thisTrans;
    private void Start()
    {
        thisTrans = transform;
    }
    Vector3 lastPos;
    Vector3 euler;
    Quaternion q;
    [SerializeField]
    float DeltaModifier = 0.1f;
    [SerializeField]
    float RotateStep= 0.1f;
    [SerializeField]
    float RotateSpeed = 0.1f;
    Touch t;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                lastPos = t.position;
            }
            if (t.phase == TouchPhase.Moved)
            {
                if (Mathf.Abs(t.position.x - lastPos.x) > DeltaModifier)
                {
                    if (t.position.x > lastPos.x)
                    {
                        euler = thisTrans.rotation.eulerAngles;
                        euler.y -= RotateStep;
                        q.eulerAngles = euler;
                        thisTrans.rotation = Quaternion.Lerp(thisTrans.rotation, q, RotateSpeed);
                    }
                    else
                    {
                        euler = thisTrans.rotation.eulerAngles;
                        euler.y += RotateStep;
                        q.eulerAngles = euler;
                        thisTrans.rotation = Quaternion.Lerp(thisTrans.rotation, q, RotateSpeed);
                    }
                    lastPos = t.position;
                }
            }
        }
    }
}
