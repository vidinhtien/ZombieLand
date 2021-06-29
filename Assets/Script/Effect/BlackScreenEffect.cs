using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenEffect : MonoBehaviour
{
    public static BlackScreenEffect instance;
    public Image im;
    private Color defaultC, tmpC, startC;
    [SerializeField]
    float blackPercent = 0.95f;
    private void Start()
    {
        MakeInstance();
        if (im == null)
            im = GetComponentInChildren<Image>();
        defaultC = new Color(0, 0, 0, 1);
        startC = new Color(0, 0, 0, 0);
        tmpC = new Color(0, 0, 0, 0);
    }
    public void On(float time)
    {
        im.color = startC;
        im.gameObject.SetActive(true);
        StartCoroutine(OnIE(time));
    }
    public void On(float time, System.Action action)
    {
        im.color = startC;
        im.gameObject.SetActive(true);
        StartCoroutine(OnIE(time, action));
    }
    private IEnumerator OnIE(float time)
    {
        float t = time / 2;
        float t0 = 0;
        while (t0 < t)
        {
            tmpC.a = Mathf.Lerp(im.color.a, blackPercent, t0 / t);
            im.color = tmpC;
            t0 += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tmpC.a = blackPercent;
        im.color = tmpC;
        t = time * 0.625f;
        t0 = 0;
        while (t0 < t)
        {
            tmpC.a = Mathf.Lerp(im.color.a, 0f, t0 / t);
            im.color = tmpC;
            t0 += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tmpC.a = 0f;
        im.color = tmpC;
        im.gameObject.SetActive(false);
    }
    private IEnumerator OnIE(float time, System.Action action)
    {
        float t = time / 2;
        float t0 = 0;
        while (t0 < t)
        {
            tmpC.a = Mathf.Lerp(im.color.a, blackPercent, t0 / t);
            im.color = tmpC;
            t0 += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tmpC.a = blackPercent;
        action?.Invoke();
        im.color = tmpC;
        t = time * 0.625f;
        t0 = 0;
        while (t0 < t)
        {
            tmpC.a = Mathf.Lerp(im.color.a, 0f, t0 / t);
            im.color = tmpC;
            t0 += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tmpC.a = 0f;
        im.color = tmpC;
        im.gameObject.SetActive(false);
    }
    public void OnlyOn(float time, System.Action action)
    {
        StartCoroutine(OnlyOnIE(time, action));
    }
    private IEnumerator OnlyOnIE(float time, System.Action action)
    {
        float t0 = 0;
        while (t0 < time)
        {
            tmpC.a = Mathf.Lerp(im.color.a, blackPercent, t0 / time);
            im.color = tmpC;
            t0 += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tmpC.a = blackPercent;
        action?.Invoke();
        im.color = tmpC;       
    }
    public void OffAll()
    {
        im.gameObject.SetActive(false);

    }
    public void Off(float time, System.Action action) 
    {
        StartCoroutine(OnlyOffIE(time, action));
    }
    private IEnumerator OnlyOffIE(float time, System.Action action)
    {
        float t0 = 0;
        while (t0 < time)
        {
            tmpC.a = Mathf.Lerp(im.color.a, 0, t0 / time);
            im.color = tmpC;
            t0 += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tmpC.a = 0;
        action?.Invoke();
        im.color = tmpC;
    }
    public void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
