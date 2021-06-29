using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance;
    private float camWidth;
    public int divideBy;
    [SerializeField]
    Transform playerTrans;
    [SerializeField]
    private Vector3 offset = new Vector3();
    [SerializeField]
    private Vector3 camPos;
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float fieldOfView;
    private Camera cam;
    Transform trans;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float smoothTime = 0.02f;
    bool calculated = false;
    public Vector3 minXZ, maxXZ;
    // Use this for initialization
    //Vector2 velocity;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        camPos = Vector3.zero;
        velocity = Vector3.zero;
        cam = Camera.main;
        fieldOfView = cam.fieldOfView;
        trans = transform;
        offset = trans.position - playerTrans.position;
        calculated = true;
    }

    float tempFloat;
    Vector3 tempV3;
    public void SetLimit(Vector3 min, Vector3 max)
    {
        minXZ = min + offset;
        maxXZ = max + offset;
        Debug.DrawLine(minXZ, maxXZ, Color.white, 5f);
    }
    //private void FixedUpdate()
    //{

    //}
    [SerializeField]
    Transform CamDes;
    void LateUpdate()
    {
        if (GameController.Instance.IsPlaying)
        {
            tempV3 = playerTrans.position + offset;
            tempV3.x = Mathf.Clamp(tempV3.x, minXZ.x, maxXZ.x);
            tempV3.z = Mathf.Clamp(tempV3.z, minXZ.z, maxXZ.z);
            camPos.x = Mathf.SmoothStep(camPos.x, tempV3.x, moveSpeed);
            camPos.z = Mathf.SmoothStep(camPos.z, tempV3.z, moveSpeed);
            camPos.y = trans.position.y;
            trans.position = camPos;
        }
        //tempV3 = CamDes.position + offset;
        //camPos.x = Mathf.SmoothStep(camPos.x, tempV3.x, moveSpeed);
        //camPos.z = Mathf.SmoothStep(camPos.z, tempV3.z, moveSpeed);
        //camPos.y = tempV3.y;

        //else
        //{
        //    trans.position = camPos;

        //}
    }

    public void SetPosition(Vector3 pos)
    {
        if (pos.x >= minXZ.x && pos.x <= maxXZ.x)
        {
            camPos.x = pos.x;
        }
        else
        {
            if (pos.x < minXZ.x)
            {
                camPos.x = minXZ.x;
            }
            else
            {
                camPos.x = maxXZ.x;
            }
        }
        if (pos.z >= minXZ.z && pos.z <= maxXZ.z)
        {
            camPos.z = pos.z;
        }
        else
        {
            if (pos.z < minXZ.z)
            {
                camPos.z = minXZ.z;
            }
            else
            {
                camPos.z = maxXZ.z;
            }
        }
        camPos.y = trans.position.y;
        //trans.position = camPos;
    }
    public void SetPositionNow(Vector3 pos)
    {
        if (pos.x >= minXZ.x && pos.x <= maxXZ.x)
        {
            camPos.x = pos.x;
        }
        else
        {
            if (pos.x < minXZ.x)
            {
                camPos.x = minXZ.x;
            }
            else
            {
                camPos.x = maxXZ.x;
            }
        }
        if (pos.z >= minXZ.z && pos.z <= maxXZ.z)
        {
            camPos.z = pos.z;
        }
        else
        {
            if (pos.z < minXZ.z)
            {
                camPos.z = minXZ.z;
            }
            else
            {
                camPos.z = maxXZ.z;
            }
        }
        camPos.y = trans.position.y;
        trans.position = camPos;
    }

    public void CalculateOffset()
    {
        if (PlayerController.Instance != null)
            offset = PlayerController.Instance.CamOffset; /*trans.position - playerTrans.position;*/
    }
    public void Zoom(float value)
    {
        bool isOut = false;
        if (value > fieldOfView)
        {
            isOut = true;
        }
        else
        {
            isOut = false;
        }
        StartCoroutine(ZoomTo(value, isOut));
    }

    private IEnumerator ZoomTo(float value, bool isZoomOut)
    {
        if (isZoomOut)
        {
            fieldOfView += 0.2f;
            cam.fieldOfView = fieldOfView;
        }
        else
        {
            fieldOfView -= 0.2f;
            cam.fieldOfView = fieldOfView;
        }
        yield return null;
        if (isZoomOut)
        {
            if (fieldOfView < value)
            {
                StartCoroutine(ZoomTo(value, isZoomOut));
            }
            else
            {
                fieldOfView = value;
                cam.fieldOfView = fieldOfView;
            }
        }
        else
        {
            if (fieldOfView > value)
            {
                StartCoroutine(ZoomTo(value, isZoomOut));
            }
            else
            {
                fieldOfView = value;
                cam.fieldOfView = fieldOfView;
            }
        }
    }
}
