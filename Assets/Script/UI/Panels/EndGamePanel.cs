using Pathfinding;
using Pathfinding.Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : BasePanel
{
    public static EndGamePanel Instance;
    [SerializeField]
    GameObject WinObject;
    [SerializeField]
    GameObject LoseObject;
    bool isWinz;
    [SerializeField]
    float timeBlackScreen = 1f;
    [SerializeField]
    Image fillChestProgress;
    [SerializeField]
    TextMeshProUGUI progressText;
    [SerializeField]
    Transform coinPosTrans;
    [SerializeField]
    TextMeshProUGUI coinText;
    [SerializeField]
    TextMeshProUGUI coinGetText;
    [SerializeField]
    GameObject X3CoinButton;
    [SerializeField]
    Transform CoinStartPos;
    bool isClicked = false;
    public Vector3 CoinPos { get { return coinPosTrans.position; } }
    float blackScreenTime = 1.5f;
    public void Active_Edited(bool isWin)
    {
        isWinz = isWin;
        isClicked = false;
        UpdateInformation();
        if (isWin)
        {
            LevelManage.Instance.PassCurrentLevel();
            EffectManage.Instance.TurnOnFirework();
            UpdateProgress();
        }
        if (anim != null)
        {
            if (isWin)
            {
                anim.Play("inWin");

            }
            else
            {
                anim.Play("inLose");
            }
        }
        else
        {
            if (panelObj == null)
            {
                panelObj = transform.GetChild(0).gameObject;
            }
            panelObj.SetActive(true);
            if (isWin)
            {
                WinObject.SetActive(true);
                LoseObject.SetActive(false);
            }
            else
            {
                WinObject.SetActive(false);
                LoseObject.SetActive(true);
            }
        }
    }
    public void Deactive_Edited()
    {
        if (anim != null)
        {
            if (isWinz)
            {
                anim.Play("outWin");

            }
            else
            {
                anim.Play("outLose");
            }
        }
        else
        {
            if (panelObj == null)
            {
                panelObj = transform.GetChild(0).gameObject;
            }
            panelObj.SetActive(false);
        }
        EffectManage.Instance.TurnOffFirework();
        StopAllCoroutines();
    }
    private void Start()
    {
        base.Start_BasePanel();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ButtonNext()
    {
        if (!isClicked)
        {
            isClicked = true;
            //Deactive_Edited();
            //UIManager.Instance.PreviousUIActive = UIManager.UiActivce.EndGame;
            //UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Playing;
            //BlackScreenEffect.instance.On(timeBlackScreen, LevelManage.Instance.PlayNextLevel);
            //PlayingPanel.Instance.Active();
            ButtonHome();
            //SoundManage.Instance.Play_PlayButtonClick();
        }
    }

    public void ButtonSkip()
    {

        

    }
    private void SkipAction()
    {
        SpawnerV2.Instance.OffAll();
        Deactive_Edited();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.EndGame;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Playing;
        LevelManage.Instance.PlayNextLevel();
        PlayingPanel.Instance.Active();
        SoundManage.Instance.Play_ButtonClick();
    }

    public void ButtonHome()
    {
        SpawnerV2.Instance.OffAll();
        HomePanel.Instance.Active();
        LevelManage.Instance.DestroyCurrentLevel();
        Deactive_Edited();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.EndGame;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Home;
        SoundManage.Instance.Play_ButtonClick();
    }

    public void ButtonReplay()
    {
        if (!isClicked)
        {
            isClicked = true;
            BlackScreenEffect.instance.On(blackScreenTime, ReplayAction);
            SoundManage.Instance.Play_PlayButtonClick();
        }
    }
    public void X3Coin()
    {
        
    }
    private void X3CoinAction()
    {
        coinGetText.text = "+" + (3 * GameController.Instance.goldGetInLevel);
        CoinManage.AddGem(2 * GameController.Instance.goldGetInLevel);
        //CoinAddEffectUI.Instance.ShowEffect(15, CoinStartPos.position, coinText.transform.position);
        CoinAddEffectUI.Instance.ShowEffectNoAppear(15, CoinStartPos.position, coinText.transform.position);
        X3CoinButton.SetActive(false);
    }

    private void ReplayAction()
    {
        SpawnerV2.Instance.OffAll();
        LevelManage.Instance.Replay();
        GameController.Instance.Replay();
        PlayingPanel.Instance.Active();
        Deactive_Edited();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.EndGame;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Playing;
    }
    public void ButtonRevive()
    {
        
    }
    private void ReviveAction()
    {
        SoundManage.Instance.Play_Revive();
        PlayingPanel.Instance.Active();
        Deactive_Edited();
        GameController.Instance.ContinueGame();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.EndGame;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Playing;
        PlayerController.Instance.Setup();
        EffectOnPlayerManage.Instance.BoostProtect(5f);
    }
    public void UpdateProgress()
    {
        float progress = ProgressComponent.Instance.GetProgress();
        if (progress >= 1f)
        {
            if (ProgressComponent.Instance.CheckGetGoal())
            {
                ProgressComponent.Instance.AchieveGoal();
            }
            else
            {

            }
        }
        if (fillChestProgress.fillAmount < progress)
        {
            StartCoroutine(UpdateProgressIE(progress, 1f));
        }
    }
    private IEnumerator UpdateProgressIE(float progress, float time)
    {
        float tmp = fillChestProgress.fillAmount;
        float percent;
        while (!fillChestProgress.IsActive())
        {
            yield return new WaitForEndOfFrame();
        }
        float t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            tmp = Mathf.Lerp(tmp, progress, t / time);
            fillChestProgress.fillAmount = tmp;
            percent = tmp * 10000;
            percent = percent / 100;
            progressText.text = string.Empty + (int)percent + "%";
            yield return new WaitForEndOfFrame();
        }
        fillChestProgress.fillAmount = progress;
        percent = tmp * 10000;
        percent = percent / 100;
        progressText.text = string.Empty + (int)percent + "%";
        if (progress >= 1f)
        {
            t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            fillChestProgress.fillAmount = 0f;
            progressText.text = "0%";
        }
    }
    private void UpdateInformation()
    {
        coinText.text = string.Empty + CoinManage.GetGem();
        CoinManage.AddGem(GameController.Instance.goldGetInLevel);
        coinGetText.text = "+" + GameController.Instance.goldGetInLevel;
        if (isWinz)
        {
            StartCoroutine(WaitShowCoin());
        }
    }
    private IEnumerator WaitShowCoin()
    {
        while (!coinGetText.isActiveAndEnabled)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.2f);
        int coin = GameController.Instance.goldGetInLevel;
        coin = Mathf.Clamp(coin, 0, 15);
        coinText.text = string.Empty + CoinManage.GetGem();
        //CoinAddEffectUI.Instance.ShowEffect(coin, CoinStartPos.position, coinText.transform.position);
        CoinAddEffectUI.Instance.ShowEffectNoAppear(coin, CoinStartPos.position, coinText.transform.position);

    }
}
