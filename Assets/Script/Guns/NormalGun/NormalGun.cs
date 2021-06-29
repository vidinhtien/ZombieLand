using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : BaseGun
{
    BaseBullet currentBullet;

    private void Update()
    {
        if (cooldownTimeCount > 0)
        {
            cooldownTimeCount -= Time.deltaTime;
        }
        else
        {
            isReady = true;
        }
    }
    public override void Aim()
    {

    }

    public override void Shoot()
    {
        if (!isReady) return;
        isReady = false;
        cooldownTimeCount = cooldownTime;
        currentBullet = GetBullet();
        listBulletReady.Remove(currentBullet);
        listBulletNotReady.Add(currentBullet);
        currentBullet.Setup(bulletStartPos.position, bulletStartPos.forward);
        currentBullet.Shoot(ReuseBullet);
        currentBullet.gameObject.SetActive(true);
        SoundManage.Instance.Play_GunSound();
    }
    float angle;
    [SerializeField]
    float angleMaxAutoAim = 10f;
    public override void Shoot(Vector3[] dirs)
    {
        if (!isReady) return;
        isReady = false;
        cooldownTimeCount = cooldownTime;
        SoundManage.Instance.Play_GunSound();
        if (dirs.Length == 0)
        {
            currentBullet = GetBullet();
            listBulletReady.Remove(currentBullet);
            listBulletNotReady.Add(currentBullet);
            currentBullet.Setup(bulletStartPos.position, bulletStartPos.forward);
            currentBullet.Shoot(ReuseBullet);
            currentBullet.gameObject.SetActive(true);
        }
        for (int i = 0; i < dirs.Length; i++)
        {
            currentBullet = GetBullet();
            listBulletReady.Remove(currentBullet);
            listBulletNotReady.Add(currentBullet);
            dirs[i].y = bulletStartPos.position.y;

            Vector3 dir = dirs[i] - bulletStartPos.position;
            angle = Vector3.SignedAngle(bulletStartPos.forward, dir.normalized, Vector3.up);
            if (Mathf.Abs(angle) < angleMaxAutoAim)
            {
                currentBullet.Setup(bulletStartPos.position, dir.normalized);
            }
            else
            {
                currentBullet.Setup(bulletStartPos.position, bulletStartPos.forward);
            }
            currentBullet.gameObject.SetActive(true);
            //EffectManage.Instance.TurnOnMuzzle(bulletStartPos.position, bulletStartPos.forward, bulletStartPos);
            currentBullet.Shoot(ReuseBullet);
        }
    }
    public void ReuseBullet(BaseBullet bullet)
    {
        listBulletReady.Add(bullet);
    }
    public override void SpecialSkill()
    {

    }

}
