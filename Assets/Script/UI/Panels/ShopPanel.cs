using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : BasePanel
{
    public static ShopPanel Instance;

    private void Start()
    {
        base.Start_BasePanel();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ButtonClose()
    {
        HomePanel.Instance.Active();
        Deactive();
        UIManager.Instance.PreviousUIActive = UIManager.UiActivce.Shop;
        UIManager.Instance.CurrentUIActive = UIManager.UiActivce.Home;
        SoundManage.Instance.Play_ButtonClick();
    }


}
