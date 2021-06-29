using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManage : MonoBehaviour
{
    public static GunManage Instance;
    public List<BaseGun> ListGun;
    public List<BaseGun> ListGunAvailable;
    public int maxGunAtSameTime = 2;
    public BaseGun currentGun;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void PickUpGun(BaseGun gun)
    {
        if (ListGunAvailable.Count == maxGunAtSameTime) return;
        ListGunAvailable.Add(gun);
    }
    public void OnGunOutOfArmmor(BaseGun gun)
    {
        if (ListGunAvailable.Contains(gun))
        {
            ListGunAvailable.Remove(gun);
        }
    }
    Vector3[] listDir;
    Vector3 v3tmp;
    public void Shoot(List<Vector3> enemyTrans)
    {
        listDir = enemyTrans.ToArray();
        for(int i=0; i< listDir.Length; i++)
        {
            v3tmp = listDir[i];
            v3tmp.y = currentGun.bulletPos.y;
        }
        currentGun.Shoot(listDir);
    }
    
}
