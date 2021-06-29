using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;
    [SerializeField]
    BaseBuff[] listBuffs;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        HomePanel.Instance.UpdateUpgradeButton();
    }

    public int GetBuffLevel(int id)
    {
        if (id < 0 || id > +listBuffs.Length) return 0;
        return listBuffs[id].GetLevel();
    }

    public int GetBuffValue(int id)
    {
        if (id < 0 || id > +listBuffs.Length) return 0;
        return listBuffs[id].GetBuffValue();
    }
    public int GetBuffUpgradePrice(int id)
    {
        if (id < 0 || id > +listBuffs.Length) return 9999999;
        return listBuffs[id].GetUpgradePrice();
    }
    public void UpgradeBuff(int id)
    {
        if (id < 0 || id > +listBuffs.Length) return;
        listBuffs[id].Upgrade();
    }
    //public bool IsFullyUpgraded(int id)
    //{
    //    if (id < 0 || id > +listBuffs.Length) return true;
    //    return listBuffs[id].IsFullyUpgraded();
    //}
}
