using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static ZombieSpawner;

public class SpawnerV2 : MonoBehaviour
{
    public static SpawnerV2 Instance;
    [SerializeField]
    private GameObject ZombieNormalPrefab;
    [SerializeField]
    private List<ZombieInformation> listZombieNormalInformation;
    [SerializeField]
    private GameObject ZombieBoss1Prefab;
    [SerializeField]
    private List<ZombieInformation> listZombieBoss1Information;
    [SerializeField]
    private GameObject ZombieBigBossPrefab;
    [SerializeField]
    private List<ZombieInformation> listZombieBigBossInformation;
    [SerializeField]
    private List<GameObject> listZombieNormalSpawned;
    [SerializeField]
    private List<GameObject> listZombieBoss1Spawned;
    [SerializeField]
    private List<GameObject> listZombieBigBossSpawned;
    [SerializeField]
    LayerMask layerRaycast;
    Vector3 v3Down;
    Transform thisTrans;
    // Start is called before the first frame update

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        v3Down = Vector3.down;
        thisTrans = transform;
        listRequestSpawning = new List<int>();
    }
    public bool IsSpawning { get { return listRequestSpawning.Count > 0; } }
    Vector3 tmpV3, tmpV31;
    [SerializeField]
    float checkRayCastAround = 0.3f;
    [SerializeField]
    Vector2 stepRandom;
    List<int> listRequestSpawning;
    public void SpawnInArea(ZombieType type, Vector2Int[] typeAndAmount, float y, float minX, float maxX, float minZ, float maxZ, float step1, float step2, System.Action<List<Zombie>> callBack)
    {
        tmpV3.y = y;
        listRequestSpawning.Add(this.GetHashCode());
        List<Zombie> listSet = new List<Zombie>();
        //Debug.DrawLine(new Vector3(minX, y, minZ), new Vector3(maxX, y, maxZ), Color.red, 5f);
        float step = Random.Range(stepRandom.x, stepRandom.y);
        int currType = 0, currTypeSpawned = 0;
        bool ok = false;
        for (int z = 0; z < 20; z++)
        {
            float StartX = Random.Range(minX, Mathf.Max(minX, minX + (maxX - minX) / 2));
            float Startz = Random.Range(minZ, Mathf.Max(minZ, minZ + (maxZ - minZ) / 2));
            for (float i = StartX; i <= maxX; i += step)
            {
                step = Random.Range(step1, step2);
                Startz = Random.Range(minZ, Mathf.Max(minZ, maxZ / 2));
                for (float j = Startz; j <= maxZ; j += step)
                {
                    step = Random.Range(step1, step2);
                    tmpV3.x = Mathf.Clamp(i, minX, maxX);
                    tmpV3.z = Mathf.Clamp(j, minZ, maxZ);
                    //Debug.Log("Check " + tmpV3);
                    if (CheckIsPositionOk(tmpV3, false))
                    {
                        //Debug.Log("--------- Check OK!");
                        Zombie zom = SpawnZombie(type, typeAndAmount[currType].x, tmpV3, Vector3.zero, transform);
                        tmpV31.x = tmpV3.x + Random.Range(-15, 15);
                        tmpV31.z = tmpV3.z + Random.Range(-15, 15);
                        tmpV31.y = tmpV3.y;
                        zom.transform.LookAt(tmpV31, Vector3.up);
                        listSet.Add(zom);
                        currTypeSpawned++;
                        if (currTypeSpawned >= typeAndAmount[currType].y)
                        {
                            currType++;
                            break;
                        }
                        if (currType >= typeAndAmount.Length)
                        {
                            break;
                        }
                        //break;
                    }
                }
                if (currType >= typeAndAmount.Length)
                {
                    ok = true;
                    break;
                }
            }
            if (ok)
            {
                break;
            }
        }
        if (ok)
        {
            Debug.Log("Spawned " + type + ": Done");
        }
        else
        {
            Debug.Log("Spawned " + type + ": Failed");
        }
        callBack?.Invoke(listSet);
        listRequestSpawning.Remove(this.GetHashCode());
    }
    private bool CheckIsPositionOk(Vector3 pos, bool warning)
    {
        Ray ray;
        RaycastHit hit;
        bool ok = true;
        pos.y += 50;
        Vector3 posz = pos;
        ray = new Ray(posz, v3Down);
        //Debug.DrawRay(ray.origin, 100 * ray.direction, Color.green, 5f);
        if (Physics.Raycast(ray, out hit, 100, layerRaycast))
        {
            //Debug.Log(hit.transform.gameObject.layer);
            if (hit.transform.gameObject.layer != 8)
            {
                ok = false;
            }
        }
        else
        {
            ok = false;
        }
        if (warning)
        {
            return ok;
        }
        posz = pos;
        posz.x += checkRayCastAround;
        ray = new Ray(posz, v3Down);
        //Debug.DrawRay(ray.origin, 100 * ray.direction, Color.blue, 5f);
        if (Physics.Raycast(ray, out hit, 100, layerRaycast))
        {
            if (hit.transform.gameObject.layer != 8)
            {
                ok = false;
            }
        }
        else
        {
            ok = false;
        }
        posz = pos;
        posz.x -= checkRayCastAround;
        ray = new Ray(posz, v3Down);
        //Debug.DrawRay(ray.origin, 100 * ray.direction, Color.blue, 5f);
        if (Physics.Raycast(ray, out hit, 100, layerRaycast))
        {
            if (hit.transform.gameObject.layer != 8)
            {
                ok = false;
            }
        }
        else
        {
            ok = false;
        }
        posz = pos;
        posz.y += checkRayCastAround;
        ray = new Ray(posz, v3Down);
        //Debug.DrawRay(ray.origin, 100 * ray.direction, Color.blue, 5f);
        if (Physics.Raycast(ray, out hit, 100, layerRaycast))
        {
            if (hit.transform.gameObject.layer != 8)
            {
                ok = false;
            }
        }
        else
        {
            ok = false;
        }
        posz = pos;
        posz.y -= checkRayCastAround;
        ray = new Ray(posz, v3Down);
        //Debug.DrawRay(ray.origin, 100 * ray.direction, Color.blue, 5f);
        if (Physics.Raycast(ray, out hit, 100, layerRaycast))
        {
            if (hit.transform.gameObject.layer != 8)
            {
                ok = false;
            }
        }
        else
        {
            ok = false;
        }
        return ok;
    }
    public Zombie SpawnZombie(ZombieType typeZ, int type, Vector3 pos, Vector3 rotationEuler, Transform pr)
    {
        pos.y += 0.25f;
        bool ok = false;
        switch (typeZ)
        {
            case ZombieType.NORMAL:
                if (type < 0 || type >= listZombieNormalInformation.Count) return null;
                for (int i = 0; i < listZombieNormalSpawned.Count; i++)
                {
                    if (!listZombieNormalSpawned[i].activeSelf)
                    {
                        listZombieNormalSpawned[i].transform.position = pos;
                        listZombieNormalSpawned[i].transform.rotation = Quaternion.Euler(rotationEuler);
                        listZombieNormalSpawned[i].GetComponent<IZombie>().SetupInformation(listZombieNormalInformation[type]);
                        listZombieNormalSpawned[i].SetActive(true);
                        ok = true;
                        return listZombieNormalSpawned[i].GetComponent<Zombie>();
                    }
                }
                if (!ok)
                {
                    GameObject z = Instantiate(ZombieNormalPrefab, pos, Quaternion.Euler(rotationEuler), pr);
                    z.GetComponent<IZombie>().SetupInformation(listZombieNormalInformation[type]);
                    z.SetActive(true);
                    listZombieNormalSpawned.Add(z);
                    return z.GetComponent<Zombie>();                    
                }
                break;
            case ZombieType.BOSS1:
                if (type < 0 || type >= listZombieBoss1Information.Count) return null;
                for (int i = 0; i < listZombieBoss1Spawned.Count; i++)
                {
                    if (!listZombieBoss1Spawned[i].activeSelf)
                    {
                        listZombieBoss1Spawned[i].GetComponent<IZombie>().SetupInformation(listZombieBoss1Information[type]);
                        listZombieBoss1Spawned[i].transform.position = pos;
                        listZombieBoss1Spawned[i].transform.rotation = Quaternion.Euler(rotationEuler);
                        listZombieBoss1Spawned[i].SetActive(true);
                        ok = true;
                        return listZombieBoss1Spawned[i].GetComponent<Zombie>();
                    }
                }
                if (!ok)
                {
                    GameObject z = Instantiate(ZombieNormalPrefab, pos, Quaternion.Euler(rotationEuler), pr);
                    z.GetComponent<IZombie>().SetupInformation(listZombieBoss1Information[type]);
                    z.SetActive(true);
                    listZombieBoss1Spawned.Add(z);
                    return z.GetComponent<Zombie>();
                }
                break;
            case ZombieType.BIGBOSS:
                if (type < 0 || type >= listZombieBigBossInformation.Count) return null;
                for (int i = 0; i < listZombieBigBossSpawned.Count; i++)
                {
                    if (!listZombieBigBossSpawned[i].activeSelf)
                    {
                        listZombieBigBossSpawned[i].GetComponent<IZombie>().SetupInformation(listZombieBigBossInformation[type]);
                        listZombieBigBossSpawned[i].transform.position = pos;
                        listZombieBigBossSpawned[i].transform.rotation = Quaternion.Euler(rotationEuler);
                        listZombieBigBossSpawned[i].SetActive(true);
                        ok = true;
                        return listZombieBigBossSpawned[i].GetComponent<Zombie>();
                    }
                }
                if (!ok)
                {
                    GameObject z = Instantiate(ZombieBigBossPrefab, pos, Quaternion.Euler(rotationEuler), pr);
                    z.GetComponent<IZombie>().SetupInformation(listZombieBigBossInformation[type]);
                    z.SetActive(true);
                    listZombieBigBossSpawned.Add(z);
                    return z.GetComponent<Zombie>();
                }
                break;
        }
        return null;
    }
    public void OffAll()
    {
        for(int i=0; i<listZombieBigBossSpawned.Count; i++)
        {
            listZombieBigBossSpawned[i].SetActive(false);
            listZombieBigBossSpawned[i].GetComponent<Zombie>().Setup();
        }
        for (int i = 0; i < listZombieBoss1Spawned.Count; i++)
        {
            listZombieBoss1Spawned[i].SetActive(false);
            listZombieBoss1Spawned[i].GetComponent<Zombie>().Setup();
        }
        for (int i = 0; i < listZombieNormalSpawned.Count; i++)
        {
            listZombieNormalSpawned[i].SetActive(false);
            listZombieNormalSpawned[i].GetComponent<Zombie>().Setup();
        }
    }
}
