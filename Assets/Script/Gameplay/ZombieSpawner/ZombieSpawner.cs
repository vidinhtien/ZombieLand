using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public static ZombieSpawner Instance;
    [SerializeField]
    private List<GameObject> listZombieNormalPrefab;
    [SerializeField]
    private List<GameObject> listZombieBoss1Prefab;
    [SerializeField]
    private List<GameObject> listZombieBigBossPrefab;
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
    public enum ZombieType
    {
        NORMAL, BOSS1, BIGBOSS
    }
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
    public void SpawnInArea(ZombieType type, Vector2Int[] typeAndAmount, float y, float minX, float maxX, float minZ, float maxZ, float step1, float step2, Transform parentz, System.Action<List<Zombie>> callBack)
    {
        tmpV3.y = y;
        listRequestSpawning.Add(parentz.GetHashCode());
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
                        Zombie zom = SpawnZombie(type, typeAndAmount[currType].x, tmpV3, Vector3.zero, parentz);
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
            Debug.Log("Spawned " + parentz.name + ": Done");
        }
        else
        {
            Debug.Log("Spawned " + parentz.name + ": Failed");
        }
        callBack?.Invoke(listSet);
        listRequestSpawning.Remove(parentz.GetHashCode());
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
        switch (typeZ)
        {
            case ZombieType.NORMAL:
                if (type < 0 || type >= listZombieNormalPrefab.Count) return null;
                GameObject z = Instantiate(listZombieNormalPrefab[type], pos, Quaternion.Euler(rotationEuler), pr);
                return z.GetComponent<Zombie>();
                break;
            case ZombieType.BOSS1:
                if (type < 0 || type >= listZombieBoss1Prefab.Count) return null;
                z = Instantiate(listZombieBoss1Prefab[type], pos, Quaternion.Euler(rotationEuler), pr);
                return z.GetComponent<Zombie>();
                break;
            case ZombieType.BIGBOSS:
                if (type < 0 || type >= listZombieBigBossPrefab.Count) return null;
                z = Instantiate(listZombieBigBossPrefab[type], pos, Quaternion.Euler(rotationEuler), pr);
                return z.GetComponent<Zombie>();
                break;
        }
        return null;
    }

}
