using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManage : MonoBehaviour
{
    public GameObject simpleBulletPrefab;
    public List<Bullet> listBulletReady;
    public List<Bullet> listBulletNotReady;
    public Transform bulletStartPos;
    public Bullet currentBullet;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentBullet = GetBullet();
            listBulletReady.Remove(currentBullet);
            //currentBullet.Shoot();
            currentBullet.gameObject.SetActive(true);
            currentBullet.Setup(bulletStartPos.position, bulletStartPos.forward);
        }
    }
    private Bullet GetBullet()
    {
        if (listBulletReady.Count == 0)
        {
            Bullet a = Instantiate(simpleBulletPrefab, transform.position, Quaternion.identity, transform).GetComponent<Bullet>();
            listBulletReady.Add(a);
        }
        return listBulletReady[0];
    }
}
