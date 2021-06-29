using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnPlayerManage : MonoBehaviour
{
    public static EffectOnPlayerManage Instance;
    [SerializeField]
    float timeBoostSpeed = 10;
    float _timeBoostSpeed;
    [SerializeField]
    bool boostSpeed;
    [SerializeField]
    BaseEffect HealthEffect;
    [SerializeField]
    ProtectEffect ProtectEffect;
    [SerializeField]
    TrailRenderer SpeedEffect;
    [SerializeField]
    float PlayerDefaultSpeed;
    [SerializeField]
    float PlayerBoostedSpeed;
    public bool IsProtecting { get { return ProtectEffect.IsProtecting; } }
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Update()
    {
        if (_timeBoostSpeed > 0)
        {
            _timeBoostSpeed -= Time.deltaTime;
        }
        else
        {
            if (boostSpeed)
            {
                boostSpeed = false;
                Player.Instance.SetSpeed(PlayerDefaultSpeed);
                SpeedEffect.gameObject.SetActive(false);
            }
        }

    }
    public void BoostSpeed(float time)
    {
        if (_timeBoostSpeed <= 0)
        {
            _timeBoostSpeed = time;
        }
        else
        {
            _timeBoostSpeed = time;
        }
        Player.Instance.SetSpeed(PlayerBoostedSpeed);
        SpeedEffect.Clear();
        SpeedEffect.gameObject.SetActive(true);
        boostSpeed = true;
    }
    public void BoostProtect(float time)
    {
        ProtectEffect.Play(time);
    }
    public void BoostHealth()
    {
        HealthEffect.Stop();
        HealthEffect.Play();
    }
}
