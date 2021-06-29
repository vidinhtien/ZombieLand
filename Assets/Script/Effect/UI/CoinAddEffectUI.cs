using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAddEffectUI : MonoBehaviour
{
    public static CoinAddEffectUI Instance;
    [SerializeField]
    GameObject coinImage;
    List<GameObject> listImageCoin = new List<GameObject>();
    private bool TouchDes;
    [SerializeField]
    float distanceSquareSpawn = .7f;
    float minX, maxX, minY, maxY;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void ShowEffect(int count, Vector3 start, Vector3 end)
    {
        transform.position = start;
        if (count < 30)
        {
            count = 10;
        }
        else if (count < 50)
        {
            count = 20;
        }
        else if (count < 70)
        {
            count = 25;
        }
        else
        {
            count = 30;
        }
        minX = start.x - distanceSquareSpawn;
        maxX = start.x + distanceSquareSpawn;
        minY = start.y - distanceSquareSpawn;
        maxY = start.y + distanceSquareSpawn; ;
        Vector3 tmpScale = transform.localScale;
        transform.localScale = tmpScale * 0.5f;
        for (int i = 0; i < count; i++)
        {
            Vector3 tmp;
            tmp.x = Random.Range(minX, maxX);
            tmp.y = Random.Range(minY, maxY);
            tmp.z = start.z;
            if (i < listImageCoin.Count)
            {
                listImageCoin[i].transform.position = tmp;
                listImageCoin[i].gameObject.SetActive(true);
            }
            else
            {
                GameObject a = Instantiate(coinImage, transform);
                listImageCoin.Add(a);
                a.transform.position = tmp;
                a.SetActive(true);
                SoundManage.Instance.Play_CoinGain();
            }
            StartCoroutine(ShowAndScale(tmpScale, count, end));

        }
        //StartCoroutine(ShowIE(count, start, end));
    }
    public void ShowEffectNoAppear(int count, Vector3 start, Vector3 end)
    {
        StartCoroutine(ShowIE(count, start, end));

    }
    private IEnumerator ShowAndScale(Vector3 originalScale, int coin, Vector3 des)
    {
        Vector3 tmp = transform.localScale, desScale = originalScale * 1.4f;

        float time = 0;
        while (tmp.x < desScale.x)
        {
            time += Time.deltaTime;
            tmp = Vector3.Lerp(tmp, desScale, time / 0.1f);
            transform.localScale = tmp;
            yield return null;
        }
        time = 0;
        while (tmp.x > originalScale.x)
        {
            time += Time.deltaTime;
            tmp = Vector3.Lerp(tmp, originalScale, time / 0.1f);
            transform.localScale = tmp;
            yield return null;
        }
        //SoundManager.instance.BonusUpSound();
        yield return new WaitForSeconds(.3f);
        TouchDes = false;
        for (int i = 0; i < listImageCoin.Count; i++)
        {
            StartCoroutine(MoveOverSecond(timeCoinFly, listImageCoin[i].transform, des));
            yield return null;
        }
        yield return new WaitForSeconds(timeCoinFly);
    }
    [SerializeField]
    float timeCoinFly = 1f;
    private IEnumerator ShowIE(int count, Vector3 start, Vector3 endP)
    {
        bool ok;

        while (count > 0)
        {
            count--;
            ok = false;

            for (int i = 0; i < listImageCoin.Count; i++)
            {
                if (!listImageCoin[i].activeSelf)
                {
                    ok = true;
                    listImageCoin[i].transform.position = start;
                    listImageCoin[i].SetActive(true);
                    StartCoroutine(MoveOverSecond(timeCoinFly, listImageCoin[i].transform, endP));
                    SoundManage.Instance.Play_CoinPickUp();
                    yield return new WaitForSeconds(0.05f);
                    break;
                }
            }
            if (!ok)
            {
                GameObject a = Instantiate(coinImage, transform);
                listImageCoin.Add(a);
                a.transform.position = start;
                a.SetActive(true);
                SoundManage.Instance.Play_CoinPickUp();
                StartCoroutine(MoveOverSecond(timeCoinFly, a.transform, endP));
                yield return new WaitForSeconds(0.05f);
            }

        }
    }
    private IEnumerator MoveOverSecond(float sec, Transform moveP, Vector3 des)
    {
        float t = 0;
        while (t < sec)
        {
            t += Time.deltaTime;
            moveP.position = Vector3.Lerp(moveP.position, des, t / sec);
            if (Vector3.SqrMagnitude(des - moveP.position) < 0.15f)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        moveP.position = des;
        moveP.gameObject.SetActive(false);
    }

    public void HideAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < listImageCoin.Count; i++)
        {
            listImageCoin[i].SetActive(false);
        }
    }
}
