using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap_Dot : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField]
    private Image dot_Img;

    [SerializeField]
    List<MiniMap_DotData> miniMap_DotDatas;

    private MiniMap_Object target;

    private bool wasInit = false;

    private Vector2 miniMap_MapSize;
    private Vector2 miniMap_UISize;
    private int miniMap_DotDatas_Index = -1;
    Camera uiCam;
    public void OnCreate()
    {
        rectTransform = GetComponent<RectTransform>();
        miniMap_DotDatas = MiniMap_Controller.instance.GetMiniMap_DotDatas();
        uiCam = UIManager.Instance.GetUICame();
        wasInit = true;
        miniMap_DotDatas_Index = -1;
        Deactive();
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }
    public void Deactive()
    {
        if (this != null)
            gameObject.SetActive(false);
    }
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void Setup(Vector2 miniMap_MapSize, Vector2 miniMap_UISize)
    {
        this.miniMap_MapSize = miniMap_MapSize;
        this.miniMap_UISize = miniMap_UISize;
    }
    public void Setup(MiniMap_Object miniMap_Object)
    {
        if (!wasInit)
        {
            Deactive();
            return;
        }

        target = miniMap_Object;
        miniMap_DotDatas_Index = -1;
        Active();
        Update_Target();
    }
    public void Clear()
    {
        target = null;
        Deactive();
    }

    public void Update_Target()
    {
        if (!wasInit) return;

        if (target == null || !target.IsActive())
        {
            if (IsActive())
                Deactive();
        }
        else
        {
            if (target.ModifielData())
            {
                MiniMap_DotData miniMap_DotData = target.GetMiniMap_DotData();
                if (miniMap_DotData)
                {
                    if (target.GetMiniMap_DotDatas_Index() != miniMap_DotDatas_Index)
                    {
                        dot_Img.overrideSprite = miniMap_DotData.icon;
                        dot_Img.color = miniMap_DotData.color;

                        miniMap_DotDatas_Index = target.GetMiniMap_DotDatas_Index();
                    }
                    else
                    {

                    }

                    Update_Position_WayPos();
                }
                else
                {
                    if (miniMap_DotDatas_Index != -1 && target.CompareTag(miniMap_DotDatas[miniMap_DotDatas_Index].tag_Name))
                    {
                        Update_Position_WayPos();
                    }
                    else
                    {
                        miniMap_DotDatas_Index = -1;
                        for (int i = 0; i < miniMap_DotDatas.Count; i++)
                        {
                            if (target.CompareTag(miniMap_DotDatas[i].tag_Name))
                            {
                                miniMap_DotDatas_Index = i;

                                dot_Img.overrideSprite = miniMap_DotDatas[i].icon;
                                dot_Img.color = miniMap_DotDatas[i].color;

                                Update_Position_WayPos();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (miniMap_DotDatas_Index != -1 && target.CompareTag(miniMap_DotDatas[miniMap_DotDatas_Index].tag_Name))
                {
                    Update_Position_WayPos();
                }
                else
                {
                    miniMap_DotDatas_Index = -1;
                    for (int i = 0; i < miniMap_DotDatas.Count; i++)
                    {
                        if (target.CompareTag(miniMap_DotDatas[i].tag_Name))
                        {
                            miniMap_DotDatas_Index = i;

                            dot_Img.overrideSprite = miniMap_DotDatas[i].icon;
                            dot_Img.color = miniMap_DotDatas[i].color;
                            Update_Position_WayPos();
                            break;
                        }
                    }
                }
            }

            if (miniMap_DotDatas_Index == -1)
            {
                Deactive();
            }
        }
    }

    public void Update_Position()
    {
        Vector2 pos = rectTransform.anchoredPosition;
        pos.x = (target.GetPosition().x - MiniMap_Controller.instance.GetCenterPosition().x) / miniMap_MapSize.x * miniMap_UISize.x;
        pos.y = (target.GetPosition().z - MiniMap_Controller.instance.GetCenterPosition().z) / miniMap_MapSize.y * miniMap_UISize.y;
        rectTransform.anchoredPosition = pos;
    }

    public void Update_Position_WayPos()
    {
        //Vector3 direct = (target.GetPosition() - MiniMap_Controller.instance.GetCenterPosition()).normalized;
        //direct.y = 0;
        //float angle = Vector3.Angle(direct, Vector3.right);
        //if (direct.z > 0)
        //{

        //}
        //else
        //{
        //    angle = -angle;
        //}

        //rectTransform.localEulerAngles = new Vector3(0, 0, angle);

        float minX = dot_Img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = dot_Img.GetPixelAdjustedRect().width / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = uiCam.WorldToScreenPoint(target.GetPosition());

        if (pos.x < minX || pos.x > maxX
            || pos.y < minY || pos.y > maxY)
        {
            dot_Img.enabled = true;

            if (pos.x < minX)
            {
                pos.x = minX;

                if (pos.y < minY)
                {
                    pos.y = minY;
                    rectTransform.localEulerAngles = new Vector3(0, 0, -135);
                }
                else if (pos.y > maxY)
                {
                    pos.y = maxY;
                    rectTransform.localEulerAngles = new Vector3(0, 0, 135);
                }
                else
                {
                    rectTransform.localEulerAngles = new Vector3(0, 0, 180);
                }
            }
            else if (pos.x > maxX)
            {
                pos.x = maxX;

                if (pos.y < minY)
                {
                    pos.y = minY;
                    rectTransform.localEulerAngles = new Vector3(0, 0, -45);
                }
                else if (pos.y > maxY)
                {
                    pos.y = maxY;
                    rectTransform.localEulerAngles = new Vector3(0, 0, 45);
                }
                else
                {
                    rectTransform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (pos.y < minY)
                {
                    pos.y = minY;
                    rectTransform.localEulerAngles = new Vector3(0, 0, -90);
                }
                else if (pos.y > maxY)
                {
                    pos.y = maxY;
                    rectTransform.localEulerAngles = new Vector3(0, 0, 90);
                }
            }
        }
        else
        {
            dot_Img.enabled = false;
        }
        // camera ui
        pos = uiCam.ScreenToWorldPoint(pos);
        rectTransform.position = pos;

        Vector3 rectPos = rectTransform.anchoredPosition;
        if (rectPos.y < 150f)
            rectPos.y = 150f + 51.83f / 2f;
        rectTransform.anchoredPosition = rectPos;
    }
}
