using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffComponentUI : MonoBehaviour
{
    [SerializeField]
    int id;
    [SerializeField]
    Text levelBuffText;
    [SerializeField]
    Text priceText;
    [SerializeField]
    GameObject ButtonBuyUpgrade;
    [SerializeField]
    GameObject ButtonAds;
    [SerializeField]
    int price;
    [SerializeField]
    BaseEffect effect;
    public void UpdateInformation()
    {
        levelBuffText.text = "Level " + BuffManager.Instance.GetBuffLevel(id);
        price = BuffManager.Instance.GetBuffUpgradePrice(id);
        priceText.text = string.Empty + price;
        CheckShowButton();
    }
    public void CheckShowButton()
    {
        if (CoinManage.GetGem() < price)
        {
            ButtonAds.SetActive(true);
            ButtonBuyUpgrade.SetActive(false);
        }
        else
        {
            ButtonAds.SetActive(false);
            ButtonBuyUpgrade.SetActive(true);
        }
    }
    public void Upgrade()
    {
        if (CoinManage.GetGem() < price) return;
        CoinManage.AddGem(-1 * price);
        BuffManager.Instance.UpgradeBuff(id);
        UpdateInformation();
        HomePanel.Instance.UpdateUpgradeButton();
        if (id == 0)
        {
            EffectManage.Instance.TurnOnUpgradeDamageEffect();
        }
        else
        {
            EffectManage.Instance.TurnOnUpgradeHPEffect();
        }
        effect?.Play();
        SoundManage.Instance.Play_Upgrade();
    }

    public void ViewAds()
    {
        
    }
    private void ViewAdsReward()
    {
        BuffManager.Instance.UpgradeBuff(id);
        UpdateInformation();
        HomePanel.Instance.UpdateUpgradeButton();
        if (id == 0)
        {
            EffectManage.Instance.TurnOnUpgradeDamageEffect();
        }
        else
        {
            EffectManage.Instance.TurnOnUpgradeHPEffect();
        }
        effect?.Play();
        SoundManage.Instance.Play_Upgrade();
    }

}
