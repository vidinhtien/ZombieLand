using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel
{
    public static SettingPanel Instance;

    [SerializeField]
    private SoundButton soundButton;
    [SerializeField]
    private VibrateButton vibrateButton;
    [SerializeField]
    private MusicButton musicButton;

    private void Start()
    {
        base.Start_BasePanel();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public override void Active()
    {
        base.Active();
        Fetch();
    }

    private void Fetch()
    {
        soundButton.FetchData();
        vibrateButton.FetchData();
        musicButton.FetchData();
    }

    public void ButtonClose()
    {
        //HomePanel.Instance.Active();
        Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Setting;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Home;
        SoundManage.Instance.Play_ClickClose();
    }

    public void ButtonSound()
    {
        soundButton.OnClick();
        SoundManage.Instance.Play_ButtonClick();
    }

    public void ButonMusic()
    {
        musicButton.OnClick();
        SoundManage.Instance.Play_ButtonClick();
    }

    public void ButtonVibrate()
    {
        vibrateButton.OnClick();
        SoundManage.Instance.Play_ButtonClick();
    }
    string level = "";
    private void OnGUI()
    {
        if (GameController.Instance.isTest && IsActive)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("LEVEL: ", GUILayout.Width(100), GUILayout.Height(50));
            level = GUILayout.TextField(level, GUILayout.Width(150), GUILayout.Height(50));
            if (GUILayout.Button("APPLY", GUILayout.Width(100), GUILayout.Height(100)))
            {
                int l = int.Parse(level);
                LevelManage.Instance.currentLevel = l;
                //if (GameController.Instance.isAds)
                //{
                //    LevelManage.Instance.SetUp(1, l);
                //}
                //else
                //{
                //    LevelManage.Instance.SetUp(0, l);

                //}
            }
            GUILayout.EndHorizontal();
        }

    }
    

}
