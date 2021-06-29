using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    TextMeshPro child;
    public bool IsReady { get { return !child.gameObject.activeSelf; } }
    [SerializeField]
    Animator animator;
    Transform mainCam;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void On(int damage, Vector3 pos)
    {
        if(mainCam == null)
        {
            mainCam = UIManager.Instance.GetGameCam().transform;
        }
        child.text = string.Empty + damage;
        transform.position = pos;
        transform.LookAt(mainCam.position);
        animator.Play("dmg_on");
    }
    
}
