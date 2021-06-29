using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    List<GameObject> listObject;
    [SerializeField]
    float timeFly = 0.7f;
    [SerializeField]
    float flySpeed = 0.1f;
    public GameObject SpawnObject(Vector3 pos, Vector3 dir)
    {
        for(int i=0; i<listObject.Count; i++)
        {
            if (!listObject[i].activeSelf)
            {
                listObject[i].transform.position = pos;
                listObject[i].transform.forward = dir;
                listObject[i].SetActive(true);
                return listObject[i];
            }
        }
        GameObject a = Instantiate(prefab, pos, Quaternion.LookRotation(dir), transform);
        listObject.Add(a);
        a.SetActive(true);
        return a;
    }
    public GameObject SpawnObject(Vector3 pos)
    {
        for (int i = 0; i < listObject.Count; i++)
        {
            if (!listObject[i].activeSelf)
            {
                listObject[i].transform.position = pos;
                listObject[i].SetActive(true);
                return listObject[i];
            }
        }
        GameObject a = Instantiate(prefab, pos, Quaternion.identity, transform);
        listObject.Add(a);
        a.SetActive(true);
        return a;
    }
    public void FlyToTrans(Transform trans, System.Action callback)
    {
        StartCoroutine(FlyIE(trans, callback));
        
    }
    private IEnumerator FlyIE(Transform trans, System.Action callback)
    {
        Vector3 pos;
        for (int i = 0; i < listObject.Count; i++)
        {
            if (listObject[i].activeSelf)
            {
                pos = listObject[i].transform.position;
                pos.y += 0.5f;
                StartCoroutine(MoveOverSecond(listObject[i].transform, pos, flySpeed/2));
            }
        }
        float time = Random.Range(0.5f, 0.9f);
        yield return new WaitForSeconds(time);
        for (int i = 0; i < listObject.Count; i++)
        {
            if (listObject[i].activeSelf)
            {
                StartCoroutine(MoveOverSecond(listObject[i].transform, trans, flySpeed));
            }
        }
        yield return new WaitForSeconds(1f);
        callback?.Invoke();
    }
    public void HideAll()
    {
        for (int i = 0; i < listObject.Count; i++)
        {
            listObject[i].SetActive(false);
        }
    }
    public static IEnumerator MoveOverSecond(Transform objects, Transform des, float flySpeed)
    {
        
        float t = 0;
        while ((objects.position - des.position).sqrMagnitude > .2f)
        {
            //t += Time.deltaTime;
            objects.position = Vector3.Lerp(objects.position, des.position, flySpeed);
            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator MoveOverSecond(Transform objects, Vector3 des, float flySpeed)
    {

        float t = 0;
        while ((objects.position - des).sqrMagnitude > .2f)
        {
            //t += Time.deltaTime;
            objects.position = Vector3.Lerp(objects.position, des, flySpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
