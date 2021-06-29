using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageCallBackPopupPanel : BasePanel
{
# region Singleton
    public static MessageCallBackPopupPanel INSTACNE;
    private void Awake()
    {
        if(INSTACNE == null)
        {
            INSTACNE = this;
        }
    }
    #endregion
    private GameObject panelChild;
    System.Action<bool> callBackFunc;
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private GameObject YNObject;
    [SerializeField]
    private GameObject MObject;
    public override void Active()
    {
        SoundManage.Instance.Play_ClickOpen();
        SetYNOn(false);
        callBackFunc = null;
        base.Active();
    }
    public void Active(string question)
    {
        SoundManage.Instance.Play_ClickOpen();
        SetYNOn(false);
        callBackFunc = null;
        if (question != null)
        {
            questionText.text = question;
        }
        base.Active();
    }

    public void Active(string question, System.Action<bool> callBackFunction, bool hasYN)
    {
        SoundManage.Instance.Play_ClickOpen();
        callBackFunc = callBackFunction;
        SetYNOn(hasYN);
        if (question != null)
        {
            questionText.text = question;
        }
        base.Active();
    }
    private void SetYNOn(bool on)
    {
        if (on)
        {
            YNObject.SetActive(true);
            MObject.SetActive(false);
        }
        else
        {
            YNObject.SetActive(false);
            MObject.SetActive(true);
        }
    }

    public void ButtonYes()
    {
        Deactive();
        callBackFunc?.Invoke(true);
        SoundManage.Instance.Play_ClickClose();
    }
    public void ButtonNo()
    {
        Deactive();
        callBackFunc?.Invoke(false);
        SoundManage.Instance.Play_ClickClose();
    }
}
