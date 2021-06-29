using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManage : MonoBehaviour
{
    public static SpawnerManage Instance;
    [SerializeField]
    ObjectsSpawner GrenadeSpawner;
    [SerializeField]
    ObjectsSpawner HealthBoostSpawner;
    [SerializeField]
    ObjectsSpawner ProtectBoostSpawner;
    [SerializeField]
    ObjectsSpawner SpeedBoostSpawner;
    [SerializeField]
    ObjectsSpawner CoinSpawner;
    [SerializeField]
    float itemSpawnRate = 0.02f;
    [SerializeField]
    bool _isFlyingCoin;
    public bool IsFlyingCoin { get { return _isFlyingCoin; } }

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
    public void Spawn(int id, Vector3 pos)
    {
        switch (id)
        {
            case 0:
                SpawnHealthBoost(pos);
                break;
            case 1:
                SpawnProtectBoost(pos);
                break;
            case 2:
                SpawnSpeedBoost(pos);
                break;
            case 3:
                SpawnCoin(pos);
                break;

        }
    }
    public void SpawnGrenade(Vector3 pos, Vector3 dir)
    {
        GrenadeSpawner.SpawnObject(pos, dir).GetComponent<Bomb>().Fire();
    }
    public void SpawnHealthBoost(Vector3 pos)
    {
        HealthBoostSpawner.SpawnObject(pos);
    }
    public void SpawnProtectBoost(Vector3 pos)
    {
        ProtectBoostSpawner.SpawnObject(pos);
    }
    public void SpawnCoin(Vector3 pos)
    {
        CoinSpawner.SpawnObject(pos);
    }
    public void CoinFlyToPlayer()
    {
        CoinDrop.InitFlyToPlayer();
        _isFlyingCoin = true;
        CoinSpawner.FlyToTrans(PlayerController.Instance.transform, FlyDone);
    }
    private void FlyDone()
    {
        _isFlyingCoin = false;
    }
    float val;
    public void SpawnCoinWithRate(Vector3 pos)
    {
        val = Random.value;
        if (val <= itemSpawnRate)
        {
            SpawnItem(pos);
        }
        else
        {
            CoinSpawner.SpawnObject(pos).GetComponent<CoinDrop>().Setup();
        }
    }
    public void SpawnItem(Vector3 pos)
    {
        int rand = Random.Range(1, 4);
        switch (rand)
        {
            case 1:
                SpeedBoostSpawner.SpawnObject(pos);
                break;
            case 2:
                HealthBoostSpawner.SpawnObject(pos);
                break;
            case 3:
                ProtectBoostSpawner.SpawnObject(pos);
                break;
            default:
                CoinSpawner.SpawnObject(pos).GetComponent<CoinDrop>().Setup();
                break;
        }
    }
    public void HideAllBoost()
    {
        SpeedBoostSpawner.HideAll();
        ProtectBoostSpawner.HideAll();
        HealthBoostSpawner.HideAll();
        CoinSpawner.HideAll();
    }

    public void SpawnSpeedBoost(Vector3 pos)
    {
        SpeedBoostSpawner.SpawnObject(pos);
    }
}
