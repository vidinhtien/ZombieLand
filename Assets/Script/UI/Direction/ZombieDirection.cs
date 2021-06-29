using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieDirection : MonoBehaviour
{

    public bool IsReady { get; set; }
    RectTransform rectTrans;
    Image image;
    public void Off()
    {
        image.enabled = false;
    }
    public void On()
    {
        image.enabled = true;
    }
    private Vector3 targetPosition;
    Camera uiCam, gameCam;

    public void OnCreated()
    {
        uiCam = UIManager.Instance.GetUICame();
        gameCam = UIManager.Instance.GetGameCam();
        rectTrans = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void Setup(Vector3 pos)
    {
        targetPosition = pos;
    }
    Vector3 targetPositionScreenPoint;
    Vector3 pointerWorldPosition;
    Vector3 cappedTargetScreenPosition;
    [SerializeField]
    float borderSizeX = 20f;
    [SerializeField]
    float borderSizeY = 100f;
    bool isOffScreen;
    float minX, maxX, minY, maxY;
    private void Update()
    {
        if (IsReady) return;
        targetPositionScreenPoint = gameCam.WorldToScreenPoint(targetPosition);
        minX = borderSizeX;
        maxX = Screen.width - borderSizeX;
        minY = borderSizeY;
        maxY = Screen.height - borderSizeY;
        isOffScreen = targetPositionScreenPoint.x <= minX || targetPositionScreenPoint.x >= maxX || targetPositionScreenPoint.y <= minY || targetPositionScreenPoint.y >= maxY;
        if (isOffScreen)
        {
            image.enabled = true;
            //RotatePointerTowardsTargetPosition();
            //cappedTargetScreenPosition = targetPositionScreenPoint;
            //if (cappedTargetScreenPosition.x <= borderSizeX) cappedTargetScreenPosition.x = borderSizeX;
            //if (cappedTargetScreenPosition.x >= Screen.width - borderSizeX) cappedTargetScreenPosition.x = Screen.width - borderSizeX;
            //if (cappedTargetScreenPosition.y <= borderSizeY) cappedTargetScreenPosition.y = borderSizeY;
            //if (cappedTargetScreenPosition.y >= Screen.height - borderSizeY) cappedTargetScreenPosition.y = Screen.height - borderSizeY;
            //pointerWorldPosition = uiCam.ScreenToWorldPoint(cappedTargetScreenPosition);
            //rectTrans.position = Vector3.Lerp(rectTrans.position, pointerWorldPosition, 0.2f);
            if (targetPositionScreenPoint.x < minX || targetPositionScreenPoint.x > maxX
            || targetPositionScreenPoint.y < minY || targetPositionScreenPoint.y > maxY)
            {
                if (targetPositionScreenPoint.x < minX)
                {
                    targetPositionScreenPoint.x = minX;

                    if (targetPositionScreenPoint.y < minY)
                    {
                        targetPositionScreenPoint.y = minY;
                        rectTrans.localEulerAngles = new Vector3(0, 0, -135);
                    }
                    else if (targetPositionScreenPoint.y > maxY)
                    {
                        targetPositionScreenPoint.y = maxY;
                        rectTrans.localEulerAngles = new Vector3(0, 0, 135);
                    }
                    else
                    {
                        rectTrans.localEulerAngles = new Vector3(0, 0, 180);
                    }
                }
                else if (targetPositionScreenPoint.x > maxX)
                {
                    targetPositionScreenPoint.x = maxX;

                    if (targetPositionScreenPoint.y < minY)
                    {
                        targetPositionScreenPoint.y = minY;
                        rectTrans.localEulerAngles = new Vector3(0, 0, -45);
                    }
                    else if (targetPositionScreenPoint.y > maxY)
                    {
                        targetPositionScreenPoint.y = maxY;
                        rectTrans.localEulerAngles = new Vector3(0, 0, 45);
                    }
                    else
                    {
                        rectTrans.localEulerAngles = new Vector3(0, 0, 0);
                    }
                }
                else
                {
                    if (targetPositionScreenPoint.y < minY)
                    {
                        targetPositionScreenPoint.y = minY;
                        rectTrans.localEulerAngles = new Vector3(0, 0, -90);
                    }
                    else if (targetPositionScreenPoint.y > maxY)
                    {
                        targetPositionScreenPoint.y = maxY;
                        rectTrans.localEulerAngles = new Vector3(0, 0, 90);
                    }
                }
            }
        }
        else
        {
            Off();
        }
        targetPositionScreenPoint = uiCam.ScreenToWorldPoint(targetPositionScreenPoint);
        rectTrans.position = targetPositionScreenPoint;

        Vector3 rectPos = rectTrans.anchoredPosition;
        if (rectPos.y < 150f)
            rectPos.y = 150f + 51.83f / 2f;
        rectTrans.anchoredPosition = rectPos;
    }

    Vector3 toPosition;
    Vector3 fromPosition;
    private void RotatePointerTowardsTargetPosition()
    {
        toPosition = gameCam.WorldToScreenPoint(targetPosition);
        toPosition = uiCam.ScreenToWorldPoint(toPosition);
        fromPosition = gameCam.WorldToScreenPoint(rectTrans.position);
        fromPosition = uiCam.ScreenToWorldPoint(fromPosition);
        Vector3 dir = (toPosition - fromPosition).normalized;
        Debug.DrawRay(rectTrans.position, rectTrans.up * 10f, Color.black);
        Debug.DrawRay(rectTrans.position, dir * 10f, Color.white);
        float angle = UtilsClass.GetAngleFromVectorFloat(dir);
        rectTrans.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void Hide()
    {
        Off();
    }

    public void Show(Vector3 targetPosition)
    {
        if (IsReady) return;
        targetPositionScreenPoint = gameCam.WorldToScreenPoint(targetPosition);
        isOffScreen = targetPositionScreenPoint.x <= borderSizeX || targetPositionScreenPoint.x >= Screen.width - borderSizeX || targetPositionScreenPoint.y <= borderSizeY || targetPositionScreenPoint.y >= Screen.height - borderSizeY;
        if (isOffScreen)
        {
            On();
        }
        else
        {
            Off();
        }
        this.targetPosition = targetPosition;
    }
}
