using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZombieLevelMap : MonoBehaviour, LevelMap
{
    internal Transform ZombieDad;

    [SerializeField]
    private int zombieDefeatCount = 0;
    [SerializeField]
    Transform playerPosition;
    [SerializeField]
    AreaZombie[] listArea;
    [SerializeField]
    private TextAsset data;
    [SerializeField]
    int ZombieCount;
    [SerializeField]
    float timeDelayStartGame = 0.5f;
    [SerializeField]
    float timeDelayEndGame = 0.8f;
    [SerializeField]
    Transform maxXZ, minXZ;
    [SerializeField]
    List<ItemSpawnProperty> listItemSpawn;

    private void Start()
    {
        //byte[] dataArr = data.bytes;
        //AstarPath.active.data.DeserializeGraphs(dataArr);
    }

    public void OnFinishLevel()
    {
        GameController.Instance.EndGame();
        //MiniMap_Controller.instance.OnLevel_Clear(); 
        DirectionManage.Instance.FreeAll();
        SpawnerManage.Instance.CoinFlyToPlayer();
        StartCoroutine(FinishIE());
    }
    IEnumerator FinishIE()
    {
        yield return new WaitForSeconds(timeDelayEndGame);
        while (SpawnerManage.Instance.IsFlyingCoin)
        {
            yield return new WaitForEndOfFrame();
        }
        if (Player.Instance.isAlive)
        {
            GameController.Instance.WinLevel();
        }
        else
        {
            GameController.Instance.LoseLevel();
        }
    }
    public void OnQuitLevel()
    {

    }

    public void OnStartLevel()
    {
        ZombieDad = transform.Find("Zombies");
        playerPosition = transform.Find("PlayerPosition");
        Transform AreaDad = transform.Find("Areas");
        //MiniMap_Controller.instance.
        AstarPath.active.data.DeserializeGraphs(data.bytes);
        listArea = AreaDad.GetComponentsInChildren<AreaZombie>();
        SpawnerManage.Instance.HideAllBoost();
        if (playerPosition == null)
        {
            playerPosition = transform.Find("PlayerPosition");
        }
        if (minXZ != null && maxXZ != null)
        {
            CameraMove.Instance.SetLimit(minXZ.position, maxXZ.position);

        }
        else
        {
            Debug.Log("Level " + name + " Chua set Min Max XZ");
        }
        PlayerController.Instance.TurnOnPlayer(playerPosition.position);
        for (int i = 0; i < listArea.Length; i++)
        {
            listArea[i].OnCreated();
        }
        ZombieCount = 0;
        zombieDefeatCount = 0;

        for (int i = 0; i < listItemSpawn.Count; i++)
        {
            if (listItemSpawn[i].isRandomCount)
            {
                listItemSpawn[i].countConst = Random.Range(listItemSpawn[i].countMin, listItemSpawn[i].countMax);
                listItemSpawn[i].spawnedCount = 0;
            }
        }
        StartCoroutine(StartIE());
    }
    private IEnumerator StartIE()
    {
        yield return new WaitForSeconds(timeDelayStartGame);
        WaitSpawn();
    }
    private IEnumerator WaitSpawnIE()
    {
        while (SpawnerV2.Instance.IsSpawning)
        {
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < listArea.Length; i++)
        {
            ZombieCount += listArea[i].ZombieCount;
        }
        GameController.Instance.StartGame();
        GameController.Instance.SetTotalTargetCount(ZombieCount);
        GameController.Instance.SetTargetReached(0);
        //MiniMap_Controller.instance.OnLevel_Create(new Vector2(15, 15));
    }
    private void WaitSpawn()
    {
        StartCoroutine(WaitSpawnIE());
    }

    public Vector3 GetClosetZombiePosition(Vector3 playerPos)
    {
        Vector3 pos = listArea[0].GetClosetZombiePosition(playerPos);
        float d = Vector3.SqrMagnitude(pos - playerPos);
        float z;
        Vector3 zd;
        for (int i = 1; i < listArea.Length; i++)
        {
            zd = listArea[i].GetClosetZombiePosition(playerPos);
            z = Vector3.SqrMagnitude(zd - playerPos);
            if (z < d)
            {
                pos = zd;
                d = z;
            }
        }
        return pos;
    }

    public void OnKillZombie()
    {
        zombieDefeatCount = 0;
        for (int i = 0; i < listArea.Length; i++)
        {
            zombieDefeatCount += listArea[i].ZombieDefeatCount();
        }
        GameController.Instance.SetTargetReached(zombieDefeatCount);
        if (zombieDefeatCount >= ZombieCount && GameController.Instance.IsPlaying)
        {
            OnFinishLevel();
        }
    }
    public void OnKillZombie(Vector3 pos)
    {
        zombieDefeatCount = 0;
        for (int i = 0; i < listArea.Length; i++)
        {
            zombieDefeatCount += listArea[i].ZombieDefeatCount();
        }
        bool spawned = false;
        for (int i = 0; i < listItemSpawn.Count; i++)
        {
            if (listItemSpawn[i].spawnedCount < listItemSpawn[i].countConst)
            {
                if (Random.value <= listItemSpawn[i].rate)
                {
                    SpawnerManage.Instance.Spawn(listItemSpawn[i].id, pos);
                    listItemSpawn[i].spawnedCount++;
                    spawned = true;
                    break;
                }
            }
        }
        if (!spawned)
        {
            SpawnerManage.Instance.SpawnCoinWithRate(pos);
        }
        GameController.Instance.SetTargetReached(zombieDefeatCount);
        if (zombieDefeatCount >= ZombieCount && GameController.Instance.IsPlaying)
        {
            OnFinishLevel();
        }
    }
}
