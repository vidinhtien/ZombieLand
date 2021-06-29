using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_Object : MonoBehaviour
{
    private Transform trans;

    [SerializeField]
    private List<MiniMap_DotData> miniMap_DotDatas;

    [SerializeField]
    private bool modifielData = false;
    public bool ModifielData() { return modifielData; }

    [SerializeField]
    private int miniMap_DotDatas_Index;

    private void Start()
    {
        trans = transform;

        if (MiniMap_Controller.instance != null)
            MiniMap_Controller.instance.Object_Add(this);

        miniMap_DotDatas_Index = -1;
    }
    private void OnDestroy()
    {
        if (MiniMap_Controller.instance != null)
            MiniMap_Controller.instance.Object_Remove(this);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public Transform GetTransform()
    {
        return trans;
    }
    public Vector3 GetPosition()
    {
        return trans.position;
    }

    public MiniMap_DotData GetMiniMap_DotData()
    {
        if (miniMap_DotDatas_Index != -1 && CompareTag(miniMap_DotDatas[miniMap_DotDatas_Index].tag_Name))
        {
            return miniMap_DotDatas[miniMap_DotDatas_Index];
        }
        else
        {
            for (int i = 0; i < miniMap_DotDatas.Count; i++)
            {
                if (CompareTag(miniMap_DotDatas[i].tag_Name))
                {
                    return miniMap_DotDatas[i];
                }
            }
        }

        return null;
    }
    public int GetMiniMap_DotDatas_Index() { return miniMap_DotDatas_Index; }
}
