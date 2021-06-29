using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character: MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected bool _isAlive;
    public bool isAlive { get { return _isAlive; } }
    public abstract void TakeDamage(int damage);
    public abstract void Death();
    public abstract void OnSpawn();
}
