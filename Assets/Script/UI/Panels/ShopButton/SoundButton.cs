using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField]
    GameObject activeStatusObject;

    public void OnClick()
    {
        if (SoundManage.Instance.IsSoundOn)
        {
            if (activeStatusObject == null) return;
            SoundManage.Instance.SetSoundActive(false);
            activeStatusObject.SetActive(false);
        }
        else
        {
            if (activeStatusObject == null) return;
            SoundManage.Instance.SetSoundActive(true);
            activeStatusObject.SetActive(true);
        }
    }

    public void FetchData()
    {
        if (SoundManage.Instance.IsSoundOn)
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
