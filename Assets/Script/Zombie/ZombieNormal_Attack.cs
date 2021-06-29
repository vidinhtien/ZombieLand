using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal_Attack : MonoBehaviour
{
    [SerializeField]
    bool _didTouchPlayer;
    [SerializeField]
    bool _canCheck;
    public int damage = 55;
    public void StartAttack()
    {
        _didTouchPlayer = false;
        _canCheck = true;
    }
    public void EndAttack()
    {
        _canCheck = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!_canCheck) return;
        if (_didTouchPlayer) return;
        if (collision.transform.CompareTag("Player"))
        {
            _didTouchPlayer = true;
            EffectManage.Instance.TurnOnExplore(0, collision.contacts[0].point, Vector3.up);
            DamangeTextManage.Instance.On(damage, collision.contacts[0].point);
            collision.transform.GetComponent<Character>().TakeDamage(damage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_canCheck) return;
        if (_didTouchPlayer) return;
        if (other.CompareTag("Player"))
        {
            _didTouchPlayer = true;
            EffectManage.Instance.TurnOnExplore(0, other.bounds.center, Vector3.up);
            DamangeTextManage.Instance.On(damage, other.bounds.center);
            other.transform.GetComponent<Character>().TakeDamage(damage);
        }
    }
}
