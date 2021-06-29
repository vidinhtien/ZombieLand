using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DirectionManage : MonoBehaviour
{
    public static DirectionManage Instance;
    RectTransform rectTrans;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rectTrans = GetComponent<RectTransform>();
    }
    public ZombieDirection directionPrefab;
    [SerializeField]
    private List<ZombieDirection> listDirections;
    public ZombieDirection GetFreeDirection()
    {
        for (int i = 0; i < listDirections.Count; i++)
        {
            if (listDirections[i].IsReady)
            {
                listDirections[i].OnCreated();
                listDirections[i].IsReady = false;
                return listDirections[i];
            }
        }
        ZombieDirection dir = Instantiate(directionPrefab, transform);
        dir.OnCreated();
        dir.IsReady = false;
        listDirections.Add(dir);
        return dir;
    }
    public void FreeAll()
    {
        for (int i = 0; i < listDirections.Count; i++)
        {
            listDirections[i].Off();
            listDirections[i].IsReady = true;
        }
    }
    //private void Update()
    //{
    //    Vector2 bl = new Vector2(0, 0);
    //    Vector2 br = new Vector2(Screen.width, 0);
    //    Vector2 tl = new Vector2(0, Screen.height);
    //    Vector2 tr = new Vector2(Screen.width, Screen.height);
    //    Vector3 bl3 = UIManager.Instance.GetUICame().ScreenToWorldPoint(bl);
    //    Vector3 br3 = UIManager.Instance.GetUICame().ScreenToWorldPoint(br);
    //    Vector3 tl3 = UIManager.Instance.GetUICame().ScreenToWorldPoint(tl);
    //    Vector3 tr3 = UIManager.Instance.GetUICame().ScreenToWorldPoint(tr);
    //    Debug.DrawLine(bl3, tr3, Color.green);
    //    Debug.DrawLine(br3, tl3, Color.red);
    //}
}
