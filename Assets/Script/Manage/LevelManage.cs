using System.Collections.Generic;
using UnityEngine;
public class LevelManage : MonoBehaviour
{
    public static LevelManage Instance;

    [SerializeField]
    internal GameObject currentLevelMap;

    [SerializeField]
    internal int currentMode;

    [SerializeField]
    internal int currentLevel;

    public List<int> numberOfLevels;
    internal void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (FirstOpenController.instance.IsOpenFirst)
        {
            Init();
        }
    }

    public void Init()
    {
        for (int i = 0; i < numberOfLevels.Count; i++)
        {
            int max = numberOfLevels[i];
            for (int j = 1; j <= max; j++)
            {
                PlayerPrefs.SetInt("M_" + i + "L_" + j, 0);
            }
            PlayerPrefs.SetInt("M_" + i + "L_" + 1, 1);
        }
    }

    public void SetLevel(int lv)
    {
        currentLevel = lv;
        currentMode = 0;
    }
    public bool IsLevelOpened(int mode, int level)
    {
        int check = PlayerPrefs.GetInt("M_" + mode + "L_" + level);
        return check != 0;
    }

    public void OpenLevel(int mode, int level)
    {
        PlayerPrefs.SetInt("M_" + mode + "L_" + level, 1);
    }

    public int GetMaxLevelCanPlay(int mode)
    {
        int max = numberOfLevels[mode];
        int z = 1;
        for (int i = 1; i <= max; i++)
        {
            if (IsLevelOpened(mode, i))
            {
                z = i;
            }
        }
        Debug.Log("Max level mode " + mode + " is " + z);
        return z;
    }

    public void LoadNormalLevel()
    {
        if (GameController.Instance.isAds)
        {
            SetUp(1, 1);
        }
        else
        {
            int maxLevel = GetMaxLevelCanPlay(0);
            SetUp(currentLevel, maxLevel);
        }
    }

    public GameObject GetCurrentLevelMap()
    {
        return currentLevelMap;
    }

    public void Replay()
    {
        SetUp(currentMode, currentLevel);
    }

    public void PassCurrentLevel()
    {
        if (currentLevel < numberOfLevels[currentMode])
        {
            ProgressComponent.Instance.SetCurrent(currentLevel);
            OpenLevel(currentMode, currentLevel + 1);
            currentLevel++;
        }
    }

    public void QuitLevel()
    {
        DestroyCurrentLevel();
    }

    public void SkipLevel()
    {

        DestroyCurrentLevel();
    }
    private void AskClearData(bool check)
    {
        if (check)
        {
            Init();
            UIManager.Instance.PlayToHome();
        }
        else
        {
            UIManager.Instance.PlayToHome();
        }
    }

    public void SetUp(int mode, int level)
    {
        currentMode = mode;
        currentLevel = level;
        PlayingPanel.Instance.UpdateLevelText();
        DirectionManage.Instance?.FreeAll();
        switch (mode)
        {
            case 0:
                Debug.Log("LEVEL MANAGER: Setup mode 0 level " + level);
                DestroyCurrentLevel();
                if (level > numberOfLevels[mode])
                {
                    MessageCallBackPopupPanel.INSTACNE.Active("You played all level! \nClear data?", AskClearData, true);
                    break;
                }
                GameObject levelObj = Instantiate(Resources.Load("MapPrefabs/Level " + level, typeof(GameObject)), transform) as GameObject;
                currentLevelMap = levelObj;
                currentLevelMap.GetComponent<LevelMap>().OnStartLevel();
                break;
            case 1:
                Debug.Log("LEVEL MANAGER: Setup mode 1 level " + level);
                DestroyCurrentLevel();
                if (level > numberOfLevels[mode])
                {
                    MessageCallBackPopupPanel.INSTACNE.Active("You played all level! \nClear data?", AskClearData, true);
                    break;
                }
                levelObj = Instantiate(Resources.Load("Ads/Level " + level, typeof(GameObject)), transform) as GameObject;
                currentLevelMap = levelObj;
                currentLevelMap.GetComponent<LevelMap>().OnStartLevel();
                break;
        }
    }

    public void DestroyCurrentLevel()
    {
        if (currentLevelMap != null)
        {
            Destroy(currentLevelMap.gameObject);
        }
    }

    public void FinishLevel()
    {
        currentLevelMap.GetComponent<LevelMap>().OnFinishLevel();
    }

    public void PlayNextLevel()
    {
        currentLevel++;
        SetUp(currentMode, currentLevel);
    }

    internal string level = "";
}
