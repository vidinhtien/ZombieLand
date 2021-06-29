using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    bool isOpenned = false;
    [SerializeField]
    bool opennedAwake = false;
    [SerializeField]
    Transform cube;
    [SerializeField]
    float posYLocalOpen;
    [SerializeField]
    float posYLocalClose;
    [SerializeField]
    bool isOpening;
    [SerializeField]
    float openTime = 0.5f;
    [SerializeField]
    float closeTime = 1.5f;
    bool isUsing;
    private void Start()
    {
        Setup();

    }
    public void OpenDoor()
    {
        //anim.Play("open");
        //isOpenned = true;
        if (!isUsing || (isUsing && !isOpening))
        {
            isUsing = true;
            isOpening = true;
            StartCoroutine(OpenIE());
        }
    }
    public void Setup()
    {
        isUsing = false;
        //anim = GetComponent<Animator>();
        //if (opennedAwake)
        //{
        //    anim.Play("openD");
        //    isOpenned = false;
        //}
        //else
        //{
        //    isOpenned = true;
        //}
    }
    public void Close()
    {
        //anim.Play("close");
        //isOpenned = false;
        if (!isUsing || (isUsing && isOpening))
        {
            isUsing = true;
            isOpening = false;
            StartCoroutine(CloseIE());
        }
    }

    private IEnumerator OpenIE()
    {
        float t = 0;
        Vector3 pos = cube.localPosition;
        while (t < openTime && isOpening)
        {
            t += Time.deltaTime;
            pos.y = Mathf.Lerp(pos.y, posYLocalOpen, t / openTime);
            cube.localPosition = pos;
            if (t >= openTime)
            {
                isOpening = false;
                isOpenned = true;
                isUsing = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator CloseIE()
    {
        float t = 0;
        Vector3 pos = cube.localPosition;
        while (t < closeTime && !isOpening)
        {
            t += Time.deltaTime;
            pos.y = Mathf.Lerp(pos.y, posYLocalClose, t / closeTime);
            cube.localPosition = pos;
            if (t >= openTime)
            {
                isUsing = false;
                isOpening = true;
                isOpenned = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
