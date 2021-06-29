using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemSpawnProperty 
{
    public int id;
    public float rate;
    public int countConst;
    public int countMin;
    public int countMax;
    public bool isRandomCount;
    public int spawnedCount=0;
}
