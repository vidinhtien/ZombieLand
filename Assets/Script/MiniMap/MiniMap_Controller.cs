using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_Controller : MonoBehaviour
{
    #region Singleton
    public static MiniMap_Controller instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (miniMap_Objects == null) miniMap_Objects = new List<MiniMap_Object>();
            wasInit = false;
        }
        else if (!instance.Equals(this))
        {
            Destroy(this);
        }
    }
    #endregion

    [Header("Import")]
    [SerializeField]
    private MiniMap_UI miniMap_UI;

    [Header("Setup")]
    [SerializeField]
    private Transform center_Trans;

    [SerializeField]
    private List<MiniMap_DotData> miniMap_DotDatas;

    [Header("InGame")]
    [SerializeField]
    private List<MiniMap_Object> miniMap_Objects;

    private bool wasInit = false;
    private bool isPlaying = false;

    public void OnStart()
    {
        if (miniMap_DotDatas == null) miniMap_DotDatas = new List<MiniMap_DotData>();
        miniMap_UI.OnStart();
        wasInit = true;
        isPlaying = false;
    }

    public void Object_Add(MiniMap_Object miniMap_Object)
    {
        if (!miniMap_Objects.Contains(miniMap_Object))
        {
            miniMap_Objects.Add(miniMap_Object);

            if (wasInit)
            {
                miniMap_UI.OnObject_Add(miniMap_Object);
            }
        }
    }
    public void Object_Remove(MiniMap_Object miniMap_Object)
    {
        int index = miniMap_Objects.IndexOf(miniMap_Object);
        miniMap_Objects.Remove(miniMap_Object);

        if (wasInit)
        {
            miniMap_UI.OnObject_Remove(index);
        }
    }

    public void OnLevel_Create(Vector2 mapSize)
    {
        miniMap_UI.OnLevel_Create(mapSize);
        miniMap_UI.Update_Map();
        isPlaying = false;
    }
    public void OnLevel_Clear()
    {
        miniMap_UI.OnLevel_Clear();
        isPlaying = false;
    }
    public void OnLevel_Playing()
    {
        isPlaying = true;
    }

    private void Update()
    {
        if (wasInit && isPlaying)
        {
            miniMap_UI.Update_Map();
        }
    }

    #region Get Set
    public List<MiniMap_Object> GetMiniMap_Objects()
    {
        return miniMap_Objects;
    }
    public Vector3 GetCenterPosition()
    {
        if (center_Trans)
            return center_Trans.position;
        else
            return Vector3.zero;
    }
    public List<MiniMap_DotData> GetMiniMap_DotDatas()
    {
        return miniMap_DotDatas;
    }
    #endregion
}
