using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
    [SerializeField]
    GameObject activeStatusObject;
    public void OnClick()
    {
        if (SoundManage.Instance.IsMusicOn)
        {
            if (activeStatusObject == null) return;
            SoundManage.Instance.SetMusicActive(false);
            activeStatusObject.SetActive(false);
        }
        else
        {
            if (activeStatusObject == null) return;
            SoundManage.Instance.SetMusicActive(true);
            activeStatusObject.SetActive(true);
        }
    }
    public void FetchData()
    {
        if (SoundManage.Instance.IsMusicOn)
        {
            if (activeStatusObject == null) return;
            activeStatusObject.SetActive(true);
        }
        else
        {
            if (activeStatusObject == null) return;
            activeStatusObject.SetActive(false);
        }
    }
}
