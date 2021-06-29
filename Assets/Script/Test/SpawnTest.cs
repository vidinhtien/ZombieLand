using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public List<AreaZombie> listArea;
    public void Spawn()
    {
        for(int i=0; i<listArea.Count; i++)
        {
            while (listArea[i].transform.childCount > 0)
            {
                Destroy(listArea[i].transform.GetChild(0));
            }
        }
        for(int i=0; i< listArea.Count; i++)
        {
            listArea[i].OnCreated();
        }
    }
}
