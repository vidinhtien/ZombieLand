using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet: MonoBehaviour
{
    [SerializeField]
    protected float speed = 2f;
    [SerializeField]
    public int damage = 10;
    [SerializeField]
    protected float disableTime= 4f;
    [SerializeField]
    protected float disableTimeCount = 0;
    public abstract void Setup(Vector3 position, Vector3 dir);
    public abstract void Shoot(System.Action<BaseBullet> callBack);
    
}
