using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoost : MonoBehaviour, BoostInterface
{
    public virtual void GetEffect()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnSpawn(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
