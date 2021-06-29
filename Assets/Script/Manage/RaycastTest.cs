using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    [SerializeField]
    Transform forward, check;
    [SerializeField]
    LayerMask layer;
    private void Update()
    {
        Vector3 v1 = forward.position - transform.position;
        Vector3 v2 = check.position - transform.position;
        Debug.Log(Vector3.SignedAngle(v1.normalized, v2.normalized, Vector3.up));
    }

}
