using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrateButton: MonoBehaviour
{
    [SerializeField]
    GameObject activeStatusObject;
    public void OnClick()
    {
        if (SoundManage.Instance.IsVibrateOn)
        {
            if (activeStatusObject == null) return;
            SoundManage.Instance.SetVibrateActive(false);
            activeStatusObject.SetActive(false);
        }
        else
        {
            if (activeStatusObject == null) return;
            SoundManage.Instance.SetVibrateActive(true);
            activeStatusObject.SetActive(true);
        }
    }
    public void FetchData()
    {
        if (SoundManage.Instance.IsVibrateOn)
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
