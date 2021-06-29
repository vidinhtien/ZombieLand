using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManage : MonoBehaviour
{
    public static BoostManage Instance;
    [SerializeField]
    BaseBoost[] listBoostPrefab;
    [SerializeField]
    List<BaseBoost> listSpeedBoost;
    [SerializeField]
    List<BaseBoost> listHealthBoost;
    [SerializeField]
    List<BaseBoost> listProtectBoost;
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
    public void SpawnBoost(int id, Vector3 position)
    {
        if (id < 0 || id >= listBoostPrefab.Length)
        {
            return;
        }
        bool ok = false;
        switch (id)
        {
            case 0:
                for (int i = 0; i < listSpeedBoost.Count; i++)
                {
                    if (!listSpeedBoost[i].gameObject.activeSelf)
                    {
                        listSpeedBoost[i].OnSpawn(position);
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    BaseBoost b;
                    b = Instantiate(listBoostPrefab[id], transform);
                    b = Instantiate(listBoostPrefab[id], transform);
                    b.OnSpawn(position);
                    listSpeedBoost.Add(b);
                    ok = true;
                }
                break;
            case 1:
                for (int i = 0; i < listHealthBoost.Count; i++)
                {
                    if (!listHealthBoost[i].gameObject.activeSelf)
                    {
                        listHealthBoost[i].OnSpawn(position);
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    BaseBoost b;
                    b = Instantiate(listBoostPrefab[id], transform);
                    b = Instantiate(listBoostPrefab[id], transform);
                    b.OnSpawn(position);
                    listHealthBoost.Add(b);
                    ok = true;
                }
                break;
            case 2:
                for (int i = 0; i < listProtectBoost.Count; i++)
                {
                    if (!listProtectBoost[i].gameObject.activeSelf)
                    {
                        listProtectBoost[i].OnSpawn(position);
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    BaseBoost b;
                    b = Instantiate(listBoostPrefab[id], transform);
                    b = Instantiate(listBoostPrefab[id], transform);
                    b.OnSpawn(position);
                    listProtectBoost.Add(b);
                    ok = true;
                }
                break;
        }
        if (!ok)
        {
            Debug.LogError("Khong sinh dc boost " + id);
        }
    }

}
