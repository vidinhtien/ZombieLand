using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePanel : BasePanel
{
    public static HomePanel Instance;
    [SerializeField]
    GameObject HomeBG;
    [SerializeField]
    Transform characterPositionsTrans;
    [SerializeField]
    TMPro.TextMeshProUGUI coinText;
    [SerializeField]
    TMPro.TextMeshProUGUI currentLevelText;
    [SerializeField]
    TMPro.TextMeshProUGUI nextLevelText;
    [SerializeField]
    float timeBlackScreen = 1f;
    [SerializeField]
    BuffComponentUI[] listBuffButton;
    bool isClickStart = false;
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
        //PlayerController.Instance?.Setup();
        isClickStart = false;
        UpdateCoinText();
        UpdateLevelText();
        UpdateUpgradeButton();
        HomeBG.SetActive(true);
        base.Active();
        CheckChangeMusic();
    }

    public override void Deactive()
    {
        base.Deactive();
        HomeBG.SetActive(false);
    }
    public override void DeactiveImediately()
    {
        base.DeactiveImediately();
        HomeBG.SetActive(false);
    }

    private void DeletePlayerPreft()
    {
        PlayerPrefs.DeleteAll();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = string.Empty + CoinManage.GetGem();
        }
    }
    private void UpdateLevelText()
    {
        if (currentLevelText != null)
        {
            currentLevelText.text = string.Empty + LevelManage.Instance.GetMaxLevelCanPlay(0);
        }
        if (nextLevelText != null)
        {
            nextLevelText.text = string.Empty + (LevelManage.Instance.GetMaxLevelCanPlay(0) + 1);
        }
    }

    private void CheckChangeMusic()
    {
        switch (UIManager.Instance.PreviousUIActive)
        {
            case UIManager.UiActivce.EndGame:
            case UIManager.UiActivce.Playing:
                SoundManage.Instance.Play_HomeMusic();
                break;
            default:
                break;
        }
    }

    public void ButtonStart()
    {
        if (isClickStart) return;
        isClickStart = true;
        SoundManage.Instance.Play_GameMusic();
        BlackScreenEffect.instance.On(timeBlackScreen, StartActions);
        SoundManage.Instance.Play_PlayButtonClick();
    }
    private void StartActions()
    {
        UIManager.Instance.OnChangeToGame();
        PlayingPanel.Instance.Active();
        if (GameController.Instance.isTest)
        {
            LevelManage.Instance.Replay();
        }
        else
        {
            LevelManage.Instance.LoadNormalLevel();

        }
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Home;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Playing;
        DeactiveImediately();
    }

    public void ButtonSetting()
    {
        SettingPanel.Instance.Active();
        //Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Home;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Setting;
        SoundManage.Instance.Play_ClickOpen();
    }

    public void ButtonShop()
    {
        ShopPanel.Instance.Active();
        Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Home;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Shop;
        SoundManage.Instance.Play_ClickOpen();
    }

    public void UpdateUpgradeButton()
    {
        if (BuffManager.Instance == null) return;
        for (int i = 0; i < listBuffButton.Length; i++)
        {
            listBuffButton[i].UpdateInformation();
        }
        UpdateCoinText();
    }

}
