using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : BaseBoost
{
    bool isTouched = false;
    public static bool IsMovingToPlayer;
    public static void InitFlyToPlayer()
    {
        IsMovingToPlayer = true;
    }
    public static void InitForGame()
    {
        IsMovingToPlayer = false;
    }
    public void Setup()
    {
        isTouched = false;
    }
    public override void GetEffect()
    {
        base.GetEffect();
        //CoinManage.AddGem(1);
        DamangeTextManage.Instance.On1(1, transform.position);
        GameController.Instance.GetGold(1);
        if (!IsMovingToPlayer)
        {
            SoundManage.Instance.Play_CoinPickUp();
        }
        else
        {
                SoundManage.Instance.Play_CoinPickUpDelay();
        }

    }
    public override void OnSpawn(Vector3 pos)
    {
        base.OnSpawn(pos);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isTouched) return;
        if (!other.GetComponent<HealthComponent>().IsDead())
        {
            isTouched = true;
            GetEffect();

        }
    }
}
