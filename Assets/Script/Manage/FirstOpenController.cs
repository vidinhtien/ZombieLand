using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstOpenController : MonoBehaviour
{
    public static FirstOpenController instance;
    public int open;
    private const string DidRate = "Did player rate game";
    public void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Awake()
    {
        MakeInstance();
        open = 111;
        //PlayerPrefs.DeleteAll();
        IsGameStartTheFirstTime();
    }
    public bool IsOpenFirst { get { return open != 111; } }
    private void IsGameStartTheFirstTime()
    {
        if (!PlayerPrefs.HasKey("IsGameStartedForTheFirstTime"))
        {
            PlayerPrefs.SetInt("IsGameStartedForTheFirstTime", 0);
            open = 123;
            PlayerPrefs.SetInt(DidRate, 0);
            //GemManage.FirstOpenInit();
            CoinManage.FirstOpenInit();
            SoundManage.FirstInit();
            PrefInfo.FirstInit();
            //LifeManager.FirstInit();
            //HintManage.FirstInit();
            //WinReward.FirstInit();
            //DailyReward.FirstInit();
            //Const.FirstInit();
            //IQManager.FirstInit();
            //IAPControl.FirstInit();
        }
    }
    public static bool DidPlayerRate()
    {
        return  PlayerPrefs.GetInt(DidRate) == 1 ;
    }
    public static void PlayerRated()
    {
        PlayerPrefs.SetInt(DidRate, 1);
    }
}
