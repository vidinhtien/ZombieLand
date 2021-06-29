using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack_Check : MonoBehaviour
{
    [SerializeField]
    ZombieNormal_Attack Attack;
    public void Attack_Start()
    {
        if (!GameController.Instance.IsPlaying) return;
        Attack.gameObject.SetActive(true);
        Attack.StartAttack();
    }
    public void Attack_End()
    {
        Attack.EndAttack();
        Attack.gameObject.SetActive(false);
    }
}
