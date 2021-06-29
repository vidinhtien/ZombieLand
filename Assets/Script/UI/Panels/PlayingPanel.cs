using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayingPanel : BasePanel
{
    public static PlayingPanel Instance;
    [SerializeField]
    private Image progressFillImg;
    [SerializeField]
    private TextMeshProUGUI percentText;
    [SerializeField]
    private TextMeshProUGUI coinText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    float timeBlackScreen = 1f;
    [SerializeField]
    Transform coinPosTrans;
    public Vector3 CoinPos { get { return coinPosTrans.position; } }
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
        isClickHome = false;
        UpdateCoinText();
        base.Active();
    }
    public void FetchProgress(float progress)
    {
        progressFillImg.fillAmount = progress;
        progress = progress * 10000;
        progress = progress / 100;
        //Debug.Log("Progress: " + (int)progress);
        percentText.text = string.Empty + (int)progress + "%";
    }
    public void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level " + LevelManage.Instance.currentLevel;
        }
    }
    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = string.Empty + /*GameController.Instance.goldGetInLevel; */CoinManage.GetGem();
        }
    }

    public void ButtonPause()
    {
        PausePanel.Instance.Active();
        SoundManage.Instance.Play_ClickOpen();
        Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Playing;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Pause;
    }

    public void ButtonQuit()
    {
        SoundManage.Instance.Play_ClickClose();
        LevelManage.Instance.QuitLevel();
        GameController.Instance.QuitLevel();
        //SoundManage.Instance.Play_ButtonClick();
    }
    bool isClickHome = false;

    public void ButtonHome()
    {
        if (isClickHome) return;
        isClickHome = true;
        if (GameController.Instance.IsPlaying)
        {
            BlackScreenEffect.instance.On(timeBlackScreen, ToHomeAction);
            SoundManage.Instance.Play_ClickClose();
        }
    }
    private void ToHomeAction()
    {
        SpawnerV2.Instance.OffAll();
        HomePanel.Instance.Active();
        UIManager.Instance.OnChangeToUI();
        Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Playing;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Home;
        LevelManage.Instance.DestroyCurrentLevel();

    }
    public void ButtonReplay()
    {
        LevelManage.Instance.Replay();
        GameController.Instance.Replay();
        SoundManage.Instance.Play_ButtonClick();
    }

}
