using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    int baseHealth;
    public int BaseHealth { get { return baseHealth; } }
    public Transform _GunPos;
    public Transform _GrenadePos;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Player.Instance.ThrowNade();
        }
    }
    public void SoundFootStep()
    {
        SoundManage.Instance.Play_FootStep();
    }
}
