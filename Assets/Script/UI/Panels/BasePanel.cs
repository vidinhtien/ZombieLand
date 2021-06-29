using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour, IPanel
{
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected GameObject panelObj;
    public bool IsActive { get { return panelObj.activeSelf; } }
    private void Start()
    {
        Start_BasePanel();
    }
    protected virtual void Start_BasePanel()
    {
        anim = GetComponent<Animator>();
        panelObj = transform.GetChild(0).gameObject;
    }

    public virtual void Active()
    {
        if (anim != null)
        {
            try
            {
                anim.Play("in");
            }
            catch
            {
                if(panelObj == null)
                {
                    panelObj = transform.GetChild(0).gameObject;
                }
                panelObj.SetActive(true);
            }
        }
        else
        {
            if (panelObj == null)
            {
                panelObj = transform.GetChild(0).gameObject;
            }
            panelObj.SetActive(true);
        }
    }

    public virtual void Deactive()
    {
        if (anim != null)
        {
            try
            {
                anim.Play("out");
            }
            catch
            {
                panelObj.SetActive(false);
            }
        }
        else
        {
            panelObj.SetActive(false);
        }
    }
    public virtual void DeactiveImediately()
    {
        if (anim != null)
        {
            try
            {
                anim.Play("out1");
            }
            catch
            {
                panelObj.SetActive(false);
            }
        }
        else
        {
            panelObj.SetActive(false);
        }
    }
}
