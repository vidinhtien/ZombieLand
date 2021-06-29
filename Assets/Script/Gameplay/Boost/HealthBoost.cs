using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : BaseBoost
{
    [SerializeField]
    int healthPoint = 200;
    public override void GetEffect()
    {
        base.GetEffect();

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
            other.GetComponent<HealthComponent>().Heal(healthPoint);
            EffectOnPlayerManage.Instance.BoostHealth();
        }
    }
}
