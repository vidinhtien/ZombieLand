using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamangeTextManage : MonoBehaviour
{
    public static DamangeTextManage Instance; 
    [SerializeField]
    DamageText prefab;
    [SerializeField]
    List<DamageText> listText;
    [SerializeField]
    DamageText prefab1;
    [SerializeField]
    List<DamageText> listText1;
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void On(int damage, Vector3 pos)
    {
        for(int i=0; i<listText.Count; i++)
        {
            if (listText[i].IsReady)
            {
                listText[i].On(damage, pos);
                return;
            }
        }
        DamageText t = Instantiate(prefab, transform);
        listText.Add(t);
        t.On(damage, pos);
        return;
    }
    public void On1(int damage, Vector3 pos)
    {
        for (int i = 0; i < listText1.Count; i++)
        {
            if (listText1[i].IsReady)
            {
                listText1[i].On(damage, pos);
                return;
            }
        }
        DamageText t = Instantiate(prefab1, transform);
        listText1.Add(t);
        t.On(damage, pos);
        return;
    }
}
