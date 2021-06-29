using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    BasePanel homePanel;
    [SerializeField]
    BasePanel playingPanel;
    [SerializeField]
    BasePanel shopPanel;
    [SerializeField]
    BasePanel pausePanel;
    [SerializeField]
    BasePanel settingPanel;
    [SerializeField]
    BasePanel endPanel;
    [SerializeField]
    GameObject LightGame;
    [SerializeField]
    GameObject LightUI;
    [SerializeField]
    Canvas UICanvas;
    [SerializeField]
    Camera GameCam;
    [SerializeField]
    Camera UICam;
    [SerializeField]
    CameraMove camMoveScript;
    [SerializeField]
    PlayerController player;
    [SerializeField]
    GameObject PleaseWaitPanel;
    public static UIManager Instance;
    public UiActivce CurrentUIActive { get; set; }
    public UiActivce PreviousUIActive { get; set; }

    [SerializeField]

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Init();
    }
    private void Init()
    {
        homePanel.Active();
        PreviousUIActive = UIManager.UiActivce.Home;
        CurrentUIActive = UiActivce.Home;
        LightUIOn();
        SoundManage.Instance.Play_HomeMusic();
    }

    public enum UiActivce
    {
        Home = 1, Playing = 2, Setting = 3, Shop = 4, EndGame = 5, Pause = 6
    }
    public void PlayToHome()
    {
        PlayingPanel.Instance.Deactive();
        HomePanel.Instance.Active();
        OnChangeToUI();
        PreviousUIActive = UIManager.UiActivce.Playing;
        CurrentUIActive = UiActivce.Home;
    }
    public Camera GetGameCam()
    {
        return GameCam;
    }
    public Camera GetUICame()
    {
        return UICam;
    }
    public void OnChangeToUI()
    {
        player.TurnOffPlayer();
        LightUIOn();
    }
    public void OnChangeToGame()
    {
        player.gameObject.SetActive(true);
        //camMoveScript.SetPosition(player.CamPosition);
        camMoveScript.CalculateOffset();
        LightGameOn();
    }
    private void LightUIOn()
    {
        LightGame.SetActive(false);
        LightUI.SetActive(true);
    }
    private void LightGameOn()
    {
        LightUI.SetActive(false);
        LightGame.SetActive(true);
    }
    public void PleaseWaitPanelOn()
    {
        PleaseWaitPanel.SetActive(true);
    }
    public void PleaseWaitPanelOff()
    {
        PleaseWaitPanel.SetActive(false);

    }
    //private void Update()
    //{
    //    Vector2 bl = new Vector2(0, 0);
    //    Vector2 br = new Vector2(Screen.width, 0);
    //    Vector2 tl = new Vector2(0, Screen.height);
    //    Vector2 tr = new Vector2(Screen.width, Screen.height);
    //    Vector3 bl3 = UICam.ScreenToWorldPoint(bl);
    //    Vector3 br3 = UICam.ScreenToWorldPoint(br);
    //    Vector3 tl3 = UICam.ScreenToWorldPoint(tl);
    //    Vector3 tr3 = UICam.ScreenToWorldPoint(tr);
    //    Debug.DrawLine(bl3, tr3, Color.green);
    //    Debug.DrawLine(br3, tl3, Color.red);
    //}
}
