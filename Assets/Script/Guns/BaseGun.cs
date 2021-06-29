using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    [SerializeField]
    protected float maxDistanceCanShoot = 5f;
    public float MaxDistanceCanShoot { get { return maxDistanceCanShoot; } }
    [SerializeField]
    protected int numberOfTargetAtSameTime = 2;
    public int NumberOfTargetAtSameTime { get { return numberOfTargetAtSameTime; } }
    [SerializeField]
    protected float cooldownTime = 0.2f;
    [SerializeField]
    protected float cooldownTimeCount = 0f;
    [SerializeField]
    protected Transform bulletStartPos;
    public Vector3 bulletPos { get { return bulletStartPos.position; } }
    [SerializeField]
    protected int bulletCountDefault = 500;
    public int BulletCountDefault { get { return bulletCountDefault; } }
    public int BulletCount { get; set; }
    [SerializeField]
    protected bool isReady = false;
    [SerializeField]
    protected BaseBullet BulletPrefab;
    [SerializeField]
    protected List<BaseBullet> listBulletReady;
    [SerializeField]
    protected List<BaseBullet> listBulletNotReady;
    [SerializeField]
    int damageBuffOfGun = 0;
    
    public abstract void Aim();
    public abstract void Shoot();
    public abstract void Shoot(Vector3[] dirs);
    public abstract void SpecialSkill();
    public BaseBullet GetBullet()
    {
        if (listBulletReady.Count == 0)
        {
            BaseBullet a = Instantiate(BulletPrefab, transform.position, Quaternion.identity, transform).GetComponent<BaseBullet>();
            a.damage = damageBuffOfGun + BuffManager.Instance.GetBuffValue(0);
            listBulletReady.Add(a);
        }
        listBulletReady[0].damage = damageBuffOfGun + BuffManager.Instance.GetBuffValue(0);
        return listBulletReady[0];
    }
}
