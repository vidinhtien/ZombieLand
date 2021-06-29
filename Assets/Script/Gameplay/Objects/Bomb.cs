using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    LayerMask bombAffectLayer;
    [SerializeField]
    float radius = 4f;
    [SerializeField]
    float force = 5f;
    [SerializeField]
    int damage = 200;
    [SerializeField]
    Rigidbody rgbd;
    [SerializeField]
    Vector3 offset = new Vector3(0, 0.7f, 0);
    //private void Start()
    //{
    //    Fire();
    //}
    //private void OnEnable()
    //{
    //    Fire();
    //}
    public void Fire()
    {
        rgbd.velocity = Vector3.zero;
        rgbd.AddForce(force * transform.forward);
    }
    public void Setup(Vector3 pos, Vector3 dir)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        rgbd.AddForce(force * dir);
    }
    public void Explore()
    {
        EffectManage.Instance.TurnOnBomb(transform.position);
        try
        {
            Collider[] listCollider = Physics.OverlapSphere(transform.position + offset, radius, bombAffectLayer);
            if (listCollider.Length > 0)
            {
                for(int i=0; i<listCollider.Length; i++)
                {
                    try
                    {
                        listCollider[i].GetComponent<Character>().TakeDamage(damage);
                    }
                    catch
                    {

                    }
                }
            }
        }
        catch { }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Explore();
        gameObject.SetActive(false);
    }
}
