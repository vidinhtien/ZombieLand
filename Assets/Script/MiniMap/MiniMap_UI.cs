using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap_UI : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField]
    private List<MiniMap_Object> miniMap_Objects;

    [Header("Import")]
    [SerializeField]
    private MiniMap_Dot miniMap_Dot_Prf;
    [Header("InGame")]
    [SerializeField]
    private List<MiniMap_Dot> miniMap_Dots;
    [SerializeField]
    private Vector2 mapSize;

    public void OnStart()
    {
        rectTransform = GetComponent<RectTransform>();
        miniMap_Objects = MiniMap_Controller.instance.GetMiniMap_Objects();

        for (int i = 0; i < transform.childCount; i++)
        {
            MiniMap_Dot miniMap_Dot = transform.GetChild(i).GetComponent<MiniMap_Dot>();
            if (miniMap_Dot)
            {
                miniMap_Dot.OnCreate();
                miniMap_Dots.Add(miniMap_Dot);
            }
        }

        int count = miniMap_Objects.Count - miniMap_Dots.Count;
        for (int i = 0; i < count; i++)
        {
            MiniMap_Dot miniMap_Dot = Instantiate(miniMap_Dot_Prf, rectTransform);
            miniMap_Dot.OnCreate();
            miniMap_Dots.Add(miniMap_Dot);
        }
    }

    public void Update_Map()
    {
        for (int i = 0; i < miniMap_Dots.Count; i++)
        {
            miniMap_Dots[i].Update_Target();
        }
    }

    public void OnObject_Add(MiniMap_Object miniMap_Object)
    {
        MiniMap_Dot miniMap_Dot = Instantiate(miniMap_Dot_Prf, rectTransform);
        miniMap_Dot.OnCreate();
        miniMap_Dots.Add(miniMap_Dot);
        miniMap_Dot.Setup(mapSize, rectTransform.sizeDelta);
        miniMap_Dot.Setup(miniMap_Object);
    }
    public void OnObject_Remove(int index)
    {
        if (index >= 0 && index < miniMap_Dots.Count)
            miniMap_Dots[index].Clear();
    }

    public void OnLevel_Create(Vector2 mapSize)
    {
        this.mapSize = mapSize;

        for (int i = 0; i < miniMap_Objects.Count; i++)
        {
            if (miniMap_Objects[i].IsActive())
            {
                miniMap_Dots[i].Setup(mapSize, rectTransform.sizeDelta);
                miniMap_Dots[i].Setup(miniMap_Objects[i]);
            }
            else
            {
                miniMap_Dots[i].Deactive();
            }
        }
    }
    public void OnLevel_Clear()
    {
        for (int i = 0; i < miniMap_Objects.Count; i++)
        {
            miniMap_Dots[i].Clear();
        }
    }
}
