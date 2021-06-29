using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectBoost : BaseBoost
{
    [SerializeField]
    float boostTime = 10f;
    public override void GetEffect()
    {
        base.GetEffect();
        EffectOnPlayerManage.Instance.BoostProtect(boostTime);
    }
    public override void OnSpawn(Vector3 pos)
    {
        base.OnSpawn(pos);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<HealthComponent>().IsDead())
        {
            GetEffect();
        }
    }
}
