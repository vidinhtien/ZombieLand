using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField]
    LevelManage levelManager;
    [SerializeField]
    bool _isPlaying;
    public bool IsPlaying { get { return _isPlaying; } }
    public bool isTest;
    public bool isAds;
    [SerializeField]
    GameObject PlayerObject;
    [SerializeField]
    private int totalTargetCount;
    [SerializeField]
    private int targetReachedCount;
    [SerializeField]
    GameObject JoyStick;
    public int goldGetInLevel { get; private set; }
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (isTest)
        {
            CoinManage.InitCoin(10000);
            LevelManage.Instance.SetLevel(1);
        }
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        CoinDrop.InitForGame();
        _isPlaying = true;
        totalTargetCount = 0;
        targetReachedCount = 0;
        goldGetInLevel = 0;
        JoyStick.SetActive(true);
        UIManager.Instance.OnChangeToGame();
        PlayerController.Instance.Setup();
        //MiniMap_Controller.instance.OnStart();
        PlayingPanel.Instance.FetchProgress(0);
    }
    public void GetGold(int g)
    {
        if (g > 0)
            goldGetInLevel += g;
        //PlayingPanel.Instance.UpdateCoinText();
    }
    public void SetTotalTargetCount(int total)
    {
        totalTargetCount = total;
        Debug.Log("Set total " + totalTargetCount);
    }
    public void SetTargetReached(int tg)
    {
        targetReachedCount = tg;
        //Debug.Log("Set reached " + tg);
        PlayingPanel.Instance.FetchProgress(GetProgressOfCurrentLevel());
    }
    public float GetProgressOfCurrentLevel()
    {
        if (totalTargetCount == 0)
        {
            return 0;
        }
        return (float)targetReachedCount / totalTargetCount;
    }

    public void PauseGame()
    {
        JoyStick.SetActive(false);
        _isPlaying = false;
    }

    public void Replay()
    {
        CoinDrop.InitForGame();
        _isPlaying = true;
        JoyStick.SetActive(true);
        PlayerController.Instance.Setup();
        goldGetInLevel = 0;
        totalTargetCount = 0;
        targetReachedCount = 0;
    }

    public void QuitLevel()
    {
        totalTargetCount = 0;
        targetReachedCount = 0;
        JoyStick.SetActive(false);
        _isPlaying = false;
    }
    public void ContinueGame()
    {
        _isPlaying = true;
        JoyStick.SetActive(true);
    }

    public void EndGame()
    {
        _isPlaying = false;
    }

    public void WinLevel()
    {
        //if (!_isPlaying) return;
        _isPlaying = false;
        JoyStick.SetActive(false);
        JoyStick.GetComponent<FloatingJoystick>().Off();
        Debug.Log("End game: Win");
        EndGamePanel.Instance.Active_Edited(true);
        PlayingPanel.Instance.Deactive();
        //PlayerController.Instance.TurnOffPlayer();
    }
    public void LoseLevel()
    {
        //if (!_isPlaying) return;
        _isPlaying = false;
        JoyStick.SetActive(false);
        JoyStick.GetComponent<FloatingJoystick>().Off();
        Debug.Log("End game: Lose");
        EndGamePanel.Instance.Active_Edited(false);
        PlayingPanel.Instance.Deactive();
        //PlayerController.Instance.TurnOffPlayer();
    }
    float currentTime;
    public void Update()
    {
        //#if UNITY_EDITOR
        currentTime += Time.deltaTime;
        //if (!Controller.Instance.isTest)
        //    return;
        counter += Time.deltaTime;
        frames += 1;
        if (counter >= 1.0f)
        {
            fps = (float)frames / counter;

            counter = 0.0f;
            frames = 0;
        }
        //#endif
    }
    private float counter;
    private int frames;
    private float fps;
    public string Header;
    protected virtual void OnGUI()
    {
        // Draw FPS?
        if (fps > 0.0f)
        {
            DrawText("FPS: " + fps.ToString("0"), TextAnchor.UpperLeft);
        }

        // Draw header?
        if (string.IsNullOrEmpty(Header) == false)
        {
            DrawText(Header, TextAnchor.UpperCenter, 150);
        }
    }
    // Cached GUI styles
    private static GUIStyle whiteStyle;
    private static GUIStyle blackStyle;

    private static void DrawText(string text, TextAnchor anchor, int offsetX = 15, int offsetY = 15)
    {
        if (string.IsNullOrEmpty(text) == false)
        {
            if (whiteStyle == null || blackStyle == null)
            {
                whiteStyle = new GUIStyle();
                whiteStyle.fontSize = 20;
                whiteStyle.fontStyle = FontStyle.Bold;
                whiteStyle.wordWrap = true;
                whiteStyle.normal = new GUIStyleState();
                whiteStyle.normal.textColor = Color.white;

                blackStyle = new GUIStyle();
                blackStyle.fontSize = 20;
                blackStyle.fontStyle = FontStyle.Bold;
                blackStyle.wordWrap = true;
                blackStyle.normal = new GUIStyleState();
                blackStyle.normal.textColor = Color.black;
            }

            whiteStyle.alignment = anchor;
            blackStyle.alignment = anchor;

            var sw = (float)Screen.width;
            var sh = (float)Screen.height;
            var rect = new Rect(0, 0, sw, sh);

            rect.xMin += offsetX;
            rect.xMax -= offsetX;
            rect.yMin += offsetY;
            rect.yMax -= offsetY;

            rect.x += 1;
            GUI.Label(rect, text, blackStyle);

            rect.x -= 2;
            GUI.Label(rect, text, blackStyle);

            rect.x += 1;
            rect.y += 1;
            GUI.Label(rect, text, blackStyle);

            rect.y -= 2;
            GUI.Label(rect, text, blackStyle);

            rect.y += 1;
            GUI.Label(rect, text, whiteStyle);
        }
    }
}