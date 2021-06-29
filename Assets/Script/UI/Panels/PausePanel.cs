using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : BasePanel
{
    public static PausePanel Instance;

    void Start()
    {
        base.Start_BasePanel();
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    public void ButtonContinue()
    {
        PlayingPanel.Instance.Active();
        Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Pause;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Playing;
        SoundManage.Instance.Play_PlayButtonClick();
    }

    public void ButtonClose()
    {
        PlayingPanel.Instance.Active();
        Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Pause;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Playing;
        SoundManage.Instance.Play_ButtonClick();
    }
}

