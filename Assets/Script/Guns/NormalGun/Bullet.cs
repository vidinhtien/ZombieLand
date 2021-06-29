using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseBullet
{
    private Vector3 direction;
    Transform thisTransform;
    bool isShooting = false;
    System.Action<BaseBullet> callBackFunc;
    Rigidbody rgbd;
    Vector3 moveDir;
    Transform target;
    [SerializeField]
    LayerMask layerRayCast;
    bool isSetup = false;
    [SerializeField]
    float distacnceCloset = 0.7f;
    [SerializeField]
    TrailRenderer TrailObject;
    private void Start()
    {
        thisTransform = transform;
        rgbd = GetComponent<Rigidbody>();
    }
    float movedDistance;
    float moveDistance;
    Vector3 hitPoint;
    public override void Setup(Vector3 position, Vector3 dir)
    {
        if (thisTransform == null)
        {
            thisTransform = transform;
        }
        thisTransform.position = position;
        direction = dir;
        transform.forward = dir;
        TrailObject.Clear();
        //Debug.DrawLine(thisTransform.position, dir*20, Color.green, 1f);
        UpdateTarget();
        isSetup = true;
        isShooting = true;
        rgbd = GetComponent<Rigidbody>();
        rgbd.velocity = new Vector3(0, 0, 0);
    }

    void UpdateTarget()
    {
        Ray r = new Ray(thisTransform.position, thisTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 100f, layerRayCast))
        {
            target = hit.transform;
            moveDistance = Vector3.SqrMagnitude(hit.point - thisTransform.position);
            hitPoint = hit.point;
        }
    }
    public override void Shoot(System.Action<BaseBullet> callBack)
    {
        disableTimeCount = disableTime;
        if (isSetup)
        {
            isShooting = true;
        }
        if (rgbd == null)
        {
            rgbd = GetComponent<Rigidbody>();
        }
        //rgbd.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
        //Invoke("ShootNothing", disableTime);
        callBackFunc = callBack;
        rgbd.velocity = transform.forward * speed * Time.deltaTime;
        //rgbd.AddForce(transform.forward * speed); 
    }
    Vector3 targetPos;
    //private void Update()
    //{
    //    if (isShooting)
    //    {
    //        rgbd.velocity = transform.forward * speed * Time.deltaTime;
    //        if (disableTimeCount > 0)
    //        {
    //            disableTimeCount -= Time.deltaTime;
    //        }
    //        else
    //        {
    //            ShootNothing();
    //        }
    //        if (target != null)
    //        {
    //            if (target.gameObject.activeSelf)
    //            {
    //                targetPos = target.position;
    //                targetPos.y = thisTransform.position.y;
    //                if ((targetPos - thisTransform.position).sqrMagnitude <= distacnceCloset)
    //                {
    //                    if (target.CompareTag("zombie"))
    //                    {
    //                        isShooting = false;
    //                        //DamangeTextManage.Instance.On(damage, thisTransform.position);
    //                        target.GetComponent<Character>().TakeDamage(damage);
    //                        gameObject.SetActive(false);
    //                        callBackFunc?.Invoke(this);
    //                        EffectManage.Instance.TurnOnExplore(0, thisTransform.position, thisTransform.forward * -1);

    //                    }
    //                    else
    //                    {
    //                        EffectManage.Instance.TurnOnExplore(1, transform.position, transform.forward * -1);
    //                        ShootNothing();
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                UpdateTarget();
    //            }
    //        }
    //    }
    //}
    private void Update()
    {
        if (disableTimeCount > 0)
        {
            disableTimeCount -= Time.deltaTime;
        }
        else
        {
            ShootNothing();
        }
        UpdateTarget();
    }
    private void FixedUpdate()
    {
        if (isShooting)
        {
            if (target != null)
            {
                if (target.gameObject.activeSelf)
                {
                    hitPoint.y = thisTransform.position.y;
                    if ((hitPoint - thisTransform.position).sqrMagnitude <= distacnceCloset)
                    {
                        if (target.CompareTag("zombie"))
                        {
                            isShooting = false;
                            //DamangeTextManage.Instance.On(damage, thisTransform.position);
                            target.GetComponent<Character>().TakeDamage(damage);
                            gameObject.SetActive(false);
                            callBackFunc?.Invoke(this);
                            TrailObject.Clear();
                            EffectManage.Instance.TurnOnExplore(0, hitPoint, thisTransform.forward * -1);

                        }
                        else
                        {
                            EffectManage.Instance.TurnOnExplore(1, hitPoint, transform.forward * -1);
                            ShootNothing();
                        }
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    
                }
            }
        }
    }
    private void ShootNothing()
    {
        if (!isShooting) return;
        gameObject.SetActive(false);
        isShooting = false;
        TrailObject.Clear();
        callBackFunc?.Invoke(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit " + other.name);
        if (!other.gameObject.activeSelf) return;
        if (other.CompareTag("zombie"))
        {
            if (!other.GetComponent<Character>().isAlive)
            {
                //ShootNothing();
                return;
            }
            isShooting = false;
            EffectManage.Instance.TurnOnExplore(0, transform.position, transform.forward * -1);
            other.GetComponent<Character>().TakeDamage(damage);
            TrailObject.Clear();
            gameObject.SetActive(false);
            callBackFunc?.Invoke(this);
        }
        else
        {
            EffectManage.Instance.TurnOnExplore(1, transform.position, transform.forward * -1);
            ShootNothing();
        }
    }
}
