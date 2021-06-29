using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManage : MonoBehaviour
{
    private const string gem = "GEM";

    public static int GetGem()
    {
        return PlayerPrefs.GetInt(gem);
    }
    
    public static void AddGem(int t)
    {
        int c = GetGem() + t;
        if (c < 0) return;
        PlayerPrefs.SetInt(gem, c);
    }
    public static void SetGem(int t)
    {
        if (t < 0) return;
        PlayerPrefs.SetInt(gem, t);
    }
    public static void FirstOpenInit()
    {
        SetGem(50);
    }
    public static void InitCoin(int c)
    {
        if (c >= 0)
        {
            SetGem(c);
        }
    }
}
