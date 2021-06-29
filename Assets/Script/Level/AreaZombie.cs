using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class AreaZombie : MonoBehaviour
{
    Transform playerTrans;

    public delegate void OnZombieUpdate(Vector3 playerPos);
    OnZombieUpdate onZombieUpdate;
    [SerializeField]
    ZombieSpawner.ZombieType type;
    [SerializeField]
    private Vector2Int[] ZombieAndType;
    [SerializeField]
    List<Zombie> listZombieOfArea;
    public int ZombieCount { get { return listZombieOfArea.Count; } }
    [SerializeField]
    private List<Zombie> zombies_Deactive;
    [SerializeField]
    private List<Zombie> zombies_Active;

    [SerializeField]
    bool isActive = false;
    [SerializeField]
    ZombieDirection directionToPlayer;
    [SerializeField]
    float timeCheck = 0.5f;
    float timeCheckCount = 0f;
    float distanncePlayerShotZombie = 0f;
    Transform thisTrans;
    [SerializeField]
    Vector2 stepRandom;
    [SerializeField]
    LayerMask RaycastLayerForZombie;

    public void OnCreated()
    {
        thisTrans = transform;
        zombies_Deactive = new List<Zombie>();
        zombies_Active = new List<Zombie>();
        try
        {
            distanncePlayerShotZombie = Player.Instance.DistanceShotZombie;
            directionToPlayer = DirectionManage.Instance.GetFreeDirection();
            _zomdefeatedCount = 0;
            playerTrans = PlayerController.Instance.transform;

        }
        catch { }
        isDieAllZombie = false;
        RequestSpawnZombie();

    }
    //private IEnumerator Wait(float minX,float maxX,float minZ, float maxZ,float y)
    //{
    //    while(ZombieSpawner.Instance == null)
    //    {
    //        yield return new WaitForEndOfFrame();
    //    }
    //    ZombieSpawner.Instance.SpawnInArea(ZombieAndType, y, minX, maxX, minZ, maxZ, stepRandom.x, stepRandom.y, thisTrans, Init);
    //    Debug.Log("Requested spawn");
    //}
    private void RequestSpawnZombie()
    {
        BoxCollider boxCol = GetComponent<BoxCollider>();
        Vector3 center = thisTrans.position + boxCol.center;
        Vector3 fieldSize = Vector3.Scale(boxCol.size, thisTrans.lossyScale);
        float minX, maxX, minZ, maxZ, y;
        minX = center.x - fieldSize.x / 2;
        maxX = center.x + fieldSize.x / 2;
        minZ = center.z - fieldSize.z / 2;
        maxZ = center.z + fieldSize.z / 2;
        y = thisTrans.position.y;
        //ZombieSpawner.Instance.SpawnInArea(type, ZombieAndType, y, minX, maxX, minZ, maxZ, stepRandom.x, stepRandom.y, thisTrans, Init);
        SpawnerV2.Instance.SpawnInArea(type, ZombieAndType, y, minX, maxX, minZ, maxZ, stepRandom.x, stepRandom.y, Init);
        //StartCoroutine(Wait(minX, maxX, minZ, maxZ, y));
    }

    private void Init(List<Zombie> listSet)
    {
        listZombieOfArea.AddRange(listSet);
        for (int i = 0; i < listZombieOfArea.Count; i++)
        {
            listZombieOfArea[i].OnCreate(this);
            zombies_Deactive.Add(listZombieOfArea[i]);
        }

    }
    public void OnZombieKilled(Zombie z)
    {
        onZombieUpdate -= z.OnUpdate;
        z.OutOfSight();
        zombies_Active.Remove(z);
    }
    Vector3 v3tmp, v3Tmp1;
    float timeDelayCal;
    float _timeDelayCal = 0.5f;
    [SerializeField]
    bool notOnScreen;
    float timeNotSeePlayer;
    float _timeNotSeePlayer = 2f;
    bool didCheckWhenCalculate;
    [SerializeField]
    bool isDieAllZombie;
    //[SerializeField]
    //float timeSpawnAgain = 5f;
    //[SerializeField]
    //int spawnTime = 5;
    private void Update()
    {
        //if (timeSpawnAgain > 0)
        //{
        //    timeSpawnAgain -= Time.deltaTime;
        //}
        //else
        //{
        //    if (spawnTime > 0)
        //    {
        //        spawnTime--;
        //        RequestSpawnZombie();
        //    }
        //    timeSpawnAgain = 5f;
        //}
        if (zombies_Active.Count > 0 || zombies_Deactive.Count > 0)
        {
            v3tmp = Player.Instance.RaycastPosition;
            if (timeCheckCount > 0)
            {
                timeCheckCount -= Time.deltaTime;
            }
            else
            {
                timeCheckCount = timeCheck;
                if (zombies_Active.Count == 0)
                {
                    notOnScreen = true;
                }
                else
                {
                    notOnScreen = false;
                }
                for (int i = zombies_Deactive.Count - 1; i >= 0; i--)
                {
                    if (!zombies_Deactive[i].isAlive)
                    {
                        onZombieUpdate -= zombies_Deactive[i].OnUpdate;
                        zombies_Deactive.RemoveAt(i);
                        //continue;
                    }
                    else
                    {
                        v3Tmp1 = zombies_Deactive[i].GetRaycastPoint();
                        v3tmp.y = v3Tmp1.y;
                        Player.Instance.SeeEnemy(zombies_Deactive[i]);
                        if (IsNearPlayer(v3Tmp1, v3tmp, zombies_Deactive[i].GetDistanceToSeePlayer(), RaycastLayerForZombie))
                        {
                            //zombies_Deactive[i].O
                            onZombieUpdate += zombies_Deactive[i].OnUpdate;
                            Player.Instance.SeeEnemy(zombies_Deactive[i]);
                            zombies_Active.Add(zombies_Deactive[i]);
                            zombies_Deactive.RemoveAt(i);
                        }
                        else
                        {
                            if (CheckOnScreen(v3Tmp1))
                            {
                                notOnScreen = false;
                            }
                        }
                    }
                }

                for (int i = zombies_Active.Count - 1; i >= 0; i--)
                {
                    if (!zombies_Active[i].isAlive)
                    {
                        onZombieUpdate -= zombies_Active[i].OnUpdate;
                        zombies_Active[i].OutOfSight();
                        zombies_Active.RemoveAt(i);
                        //continue;
                    }
                    else
                    {
                        v3Tmp1 = zombies_Active[i].GetRaycastPoint();
                        v3tmp.y = v3Tmp1.y;
                        Player.Instance.SeeEnemy(zombies_Active[i]);
                        if (!IsNearPlayer(v3Tmp1, v3tmp, zombies_Active[i].GetDistanceToSeePlayer(), RaycastLayerForZombie))
                        {
                            onZombieUpdate -= zombies_Active[i].OnUpdate;
                            zombies_Active[i].OutOfSight();
                            zombies_Deactive.Add(zombies_Active[i]);
                            zombies_Active.RemoveAt(i);

                        }
                        //else
                        //{
                        //    if (IsNearPlayer(zombies_Active[i].GetPosition(), v3tmp, distanncePlayerShotZombie))
                        //    {
                        //        Player.Instance.SeeEnemy(zombies_Active[i]);
                        //    }
                        //    else
                        //    {
                        //        Player.Instance.RemoveEnenmy(zombies_Active[i]);
                        //    }
                        //}
                    }
                }
                onZombieUpdate?.Invoke(v3tmp);
                didCheckWhenCalculate = true;
            }

        }
        if (!isDieAllZombie)
        {
            if (zombies_Active.Count == 0 && zombies_Deactive.Count == 0)
            {
                isDieAllZombie = true;
                try
                {
                    directionToPlayer?.Off();
                }
                catch { }
            }
            if (didCheckWhenCalculate)
            {
                if (notOnScreen)
                {
                    CalculateClosetZombieDirection();
                }
                else
                {
                    directionToPlayer?.Off();
                }
            }
            else
            {
                if (timeNotSeePlayer >= _timeNotSeePlayer)
                {
                    if (notOnScreen)
                    {
                        CalculateClosetZombieDirection();
                    }
                }
            }
        }
        else
        {
            try
            {
                directionToPlayer.IsReady = true;
                directionToPlayer.Off();

            }
            catch { }
        }
    }
    int indexZombieDirection;
    float minDisZombieDirection;
    float distanceTmp;
    Vector3 v3z;
    private void CheckAllDeactive()
    {

        for (int i = 0; i < zombies_Deactive.Count; i++)
        {

        }
    }
    private bool CheckOnScreen(Vector3 pos)
    {
        Vector2 d = UIManager.Instance.GetGameCam().WorldToViewportPoint(pos);
        if (d.x >= 0 && d.x <= 1 && d.y >= 0 && d.y <= 1)
        {
            return true;
        }
        return false;
    }
    private void CalculateClosetZombieDirection()
    {
        if (listZombieOfArea.Count == 0 || isDieAllZombie)
        {
            directionToPlayer?.Off();
            return;
        }
        Vector3 midPoint = listZombieOfArea[0].GetPosition(); ;
        for (int i = 0; i < listZombieOfArea.Count; i++)
        {
            if (listZombieOfArea[i].isAlive)
            {
                midPoint = listZombieOfArea[i].GetPosition();
                break;
            }
        }
        if (directionToPlayer != null && indexZombieDirection != -1)
        {
            //Debug.DrawLine(listZombieOfArea[indexZombieDirection].GetPosition(), playerPos, Color.white, 5f);
            directionToPlayer.Show(midPoint);
        }

    }
    public Vector3 GetClosetZombiePosition(Vector3 pos)
    {
        float dis = 99999999;
        int index = -1;
        float z;
        for (int i = 0; i < listZombieOfArea.Count; i++)
        {
            if (listZombieOfArea[i].isAlive)
            {
                z = Vector3.SqrMagnitude(pos - listZombieOfArea[i].GetPosition());
                if (z < dis)
                {
                    index = i;
                    dis = z;
                }
            }
        }
        if (index == -1)
        {
            return new Vector3(9999999, 9999999, 9999999);
        }
        return listZombieOfArea[index].GetPosition();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (isActive) return;
    //    if (other.CompareTag("Player"))
    //    {
    //        isActive = true;
    //        playerTrans = other.transform;
    //    }
    //}

    private static bool IsNearPlayer(Vector3 zPos, Vector3 playerPos, Vector2 distance, LayerMask layerz)
    {
        float diszz = Random.Range(distance.x, distance.y);
        if (Vector3.SqrMagnitude(zPos - playerPos) <= diszz)
        {
            //return true;
            Ray r = new Ray(zPos, (playerPos - zPos).normalized);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, diszz + 1, layerz))
            {
                if (hit.transform.CompareTag("Player")) return true;
                return false;
            }
        }
        return false;
    }
    private static bool IsNearPlayer(Vector3 zPos, Vector3 playerPos, float distance, LayerMask layerz)
    {
        if (Vector3.SqrMagnitude(zPos - playerPos) <= distance)
        {
            Ray r = new Ray(zPos, (playerPos - zPos).normalized);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, distance + 1, layerz))
            {
                if (hit.transform.CompareTag("Player")) return true;
                return false;
            }
        }
        return false;
    }
    int _zomdefeatedCount;
    public int ZombieDefeatCount()
    {
        _zomdefeatedCount = 0;
        for (int i = 0; i < listZombieOfArea.Count; i++)
        {
            if (!listZombieOfArea[i].isAlive)
            {
                _zomdefeatedCount++;
            }
        }
        return _zomdefeatedCount;
    }
}

