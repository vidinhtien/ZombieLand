using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionEffect : MonoBehaviour
{
    private LevelMap levelMap;

    public bool _isOn = false;
    Transform thisTrans;
    GameObject objectz;
    private void Start()
    {
        thisTrans = transform;
        objectz = transform.GetChild(0).gameObject;
    }
    public void UpdateLevelMap()
    {
        levelMap = LevelManage.Instance.GetCurrentLevelMap().GetComponent<LevelMap>();
    }
    Vector3 pos, dir;
    Quaternion qua;
    private void Update()
    {
        if (_isOn && GameController.Instance.IsPlaying)
        {
            try
            {
                pos = levelMap.GetClosetZombiePosition(thisTrans.position);
            }
            catch
            {
                UpdateLevelMap();
            }
            //Debug.DrawLine(pos, thisTrans.position, Color.green);
            dir = (pos - thisTrans.position).normalized;
            qua = Quaternion.LookRotation(dir);
            thisTrans.rotation = qua;
        }
    }
    public void On()
    {
        _isOn = true;
        if (objectz == null)
        {
            objectz = transform.GetChild(0).gameObject;
        }
        if (levelMap == null)
        {
            UpdateLevelMap();
        }
        if (!objectz.activeSelf)
        {
            objectz.SetActive(true);
        }
    }
    public void Off()
    {
        _isOn = false;
        if (objectz == null)
        {
            objectz = transform.GetChild(0).gameObject;
        }
        if (objectz.activeSelf)
        {
            objectz.SetActive(false);
        }
    }
}
