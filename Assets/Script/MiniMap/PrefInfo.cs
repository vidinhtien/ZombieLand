using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefInfo : MonoBehaviour
{
    // Start is called before the first frame update
    const string ADS = "ADS";
    public static bool IsUsingAd()
    {
        return 1 == PlayerPrefs.GetInt(ADS, 1);
    }
    public static void SetAd(bool on)
    {
        if (on)
        {
            PlayerPrefs.SetInt(ADS, 1);
        }
        else
        {
            PlayerPrefs.SetInt(ADS, 0);
        }
    }
    public static void FirstInit()
    {
        PlayerPrefs.SetInt(ADS, 1);
    }
}
