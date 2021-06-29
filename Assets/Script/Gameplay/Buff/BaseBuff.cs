using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff : MonoBehaviour
{
    [SerializeField]
    private string BuffName;
    const string NAME = "NAME";
    const string LEVEL = "LEVEL";
    const string VALUE = "VALUE";
    [SerializeField]
    int baseValue;
    [SerializeField]
    int stepValue;
    [SerializeField]
    int basePrice;
    [SerializeField]
    int stepPrice;
    private void Start()
    {
        if (FirstOpenController.instance.IsOpenFirst)
        {
            FirstInit();
        }
    }
    private void FirstInit()
    {
        PlayerPrefs.SetInt("BUFF_" + BuffName + "_" + LEVEL, 0);
        PlayerPrefs.SetInt("BUFF_" + BuffName + "_" + VALUE, baseValue);
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("BUFF_" + BuffName + "_" + LEVEL, 0);
    }
    public int GetUpgradePrice()
    {
        int lv = GetLevel();
        return basePrice + lv * stepPrice;
    }
    public int GetBuffValue()
    {
        return PlayerPrefs.GetInt("BUFF_" + BuffName + "_" + VALUE, 0);
    }
    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt("BUFF_" + BuffName + "_" + LEVEL, level);
    }
    public void SetValue(int value)
    {
        PlayerPrefs.SetInt("BUFF_" + BuffName + "_" + VALUE, value);
    }
    public void Upgrade()
    {
        int lv = GetLevel();
        lv++;
        int newVal = baseValue + stepValue * lv;
        SetValue(newVal);
        SetLevel(lv);
        Debug.Log("Upgraded " + BuffName + " :  level " + lv + " - value " + newVal);
    }
    //public bool IsFullyUpgraded()
    //{
    //    int level = GetLevel();
    //    if (level >= values.Length - 1)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}
