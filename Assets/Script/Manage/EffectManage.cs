using System.Collections.Generic;
using UnityEngine;
public class EffectManage : MonoBehaviour
{
    public static EffectManage Instance;

    [SerializeField]
    private MuzzleEffect MuzzleEffectPref;

    [SerializeField]
    private List<MuzzleEffect> listMuzzleEff;

    [SerializeField]
    private DustEffect DustEffectPref;

    [SerializeField]
    private List<DustEffect> listDustEff;

    [SerializeField]
    private DustEffect ExlporeEffectPref;

    [SerializeField]
    private List<DustEffect> listExploreEff;

    [SerializeField]
    private DustEffect ExlporeEffectPref1;
    [SerializeField]
    private List<DustEffect> listExploreEff1;
    [SerializeField]
    BaseEffect FireworkEffect;
    [SerializeField]
    DustEffect BloodEffectPrefab;
    [SerializeField]
    private List<DustEffect> listBloddEffect;
    [SerializeField]
    DustEffect BombPrefab;
    [SerializeField]
    private List<DustEffect> listBombEffect;
    [SerializeField]
    BaseEffect UpgradeHPEffect;
    [SerializeField]
    BaseEffect UpgradeDamageEffect;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void TurnOnMuzzle(Vector3 pos, Vector3 dir, Transform dad)
    {
        bool ok = false;
        for (int i = 0; i < listMuzzleEff.Count; i++)
        {
            if (!listMuzzleEff[i].gameObject.activeSelf)
            {
                listMuzzleEff[i].gameObject.SetActive(true);
                listMuzzleEff[i].transform.SetParent(dad);
                listMuzzleEff[i].transform.position = pos;
                listMuzzleEff[i].transform.forward = dir;
                listMuzzleEff[i].Play();
                ok = true;
                break;
            }
        }
        if (!ok)
        {
            MuzzleEffect a = Instantiate(MuzzleEffectPref, dad);
            listMuzzleEff.Add(a);
            a.transform.position = pos;
            a.transform.forward = dir;
            a.gameObject.SetActive(true);
            a.Play();
        }
    }

    [SerializeField]
    internal float dustOffsetY = 0.25f;

    public void TurnOnDust(Vector3 pos)
    {
        pos.y += dustOffsetY;
        bool ok = false;
        for (int i = 0; i < listDustEff.Count; i++)
        {
            if (!listDustEff[i].gameObject.activeSelf)
            {
                listDustEff[i].gameObject.SetActive(true);
                listDustEff[i].transform.SetParent(null);
                listDustEff[i].transform.position = pos;
                listDustEff[i].Play();
                ok = true;
                break;
            }
        }
        if (!ok)
        {
            DustEffect a = Instantiate(DustEffectPref, null);
            listDustEff.Add(a);
            a.transform.position = pos;
            a.gameObject.SetActive(true);
            a.Play();
        }
    }
    public void TurnOnBlood(Vector3 pos)
    {
        bool ok = false;
        for (int i = 0; i < listBloddEffect.Count; i++)
        {
            if (!listBloddEffect[i].gameObject.activeSelf)
            {
                listBloddEffect[i].gameObject.SetActive(true);
                listBloddEffect[i].transform.SetParent(null);
                listBloddEffect[i].transform.position = pos;
                listBloddEffect[i].Play();
                ok = true;
                break;
            }
        }
        if (!ok)
        {
            DustEffect a = Instantiate(BloodEffectPrefab, null);
            listBloddEffect.Add(a);
            a.transform.position = pos;
            a.gameObject.SetActive(true);
            a.Play();
        }
    }
    public void TurnOnBomb(Vector3 pos)
    {
        bool ok = false;
        for (int i = 0; i < listBombEffect.Count; i++)
        {
            if (!listBombEffect[i].gameObject.activeSelf)
            {
                listBombEffect[i].gameObject.SetActive(true);
                listBombEffect[i].transform.SetParent(null);
                listBombEffect[i].transform.position = pos;
                listBombEffect[i].Play();
                ok = true;
                break;
            }
        }
        if (!ok)
        {
            DustEffect a = Instantiate(BombPrefab, null);
            listBombEffect.Add(a);
            a.transform.position = pos;
            a.gameObject.SetActive(true);
            a.Play();
        }
    }
    /// <summary>
    /// The TurnOnExplore.
    /// </summary>
    /// <param name="type">The type: 0 - hit character, 1 - hit obstacle<see cref="int"/>.</param>
    /// <param name="pos">The pos<see cref="Vector3"/>.</param>
    /// <param name="dir">The dir<see cref="Vector3"/>.</param>
    public void TurnOnExplore(int type, Vector3 pos, Vector3 dir)
    {
        pos.y += dustOffsetY;
        bool ok = false;
        if (type == 0)
        {
            for (int i = 0; i < listExploreEff.Count; i++)
            {
                if (!listExploreEff[i].gameObject.activeSelf)
                {
                    listExploreEff[i].gameObject.SetActive(true);
                    listExploreEff[i].transform.SetParent(null);
                    listExploreEff[i].transform.forward = dir;
                    listExploreEff[i].transform.position = pos;
                    listExploreEff[i].Play();
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                DustEffect a = Instantiate(ExlporeEffectPref, null);
                listExploreEff.Add(a);
                a.transform.position = pos;
                a.transform.forward = dir;
                a.gameObject.SetActive(true);
                a.Play();
            }
        }
        else if (type == 1)
        {
            for (int i = 0; i < listExploreEff1.Count; i++)
            {
                if (!listExploreEff1[i].gameObject.activeSelf)
                {
                    listExploreEff1[i].gameObject.SetActive(true);
                    listExploreEff1[i].transform.SetParent(null);
                    listExploreEff1[i].transform.forward = dir;
                    listExploreEff1[i].transform.position = pos;
                    listExploreEff1[i].Play();
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                DustEffect a = Instantiate(ExlporeEffectPref1, null);
                listExploreEff1.Add(a);
                a.transform.position = pos;
                a.transform.forward = dir;
                a.gameObject.SetActive(true);
                a.Play();
            }
        }
    }

    [SerializeField]
    internal GameObject MuzzleEffectObject;

    public void TurnOnMuzzle()
    {
        //MuzzleEffectObject?.SetActive(true);
    }

    public void TurnOffMuzzle()
    {
        //MuzzleEffectObject?.SetActive(false);
    }
    public void TurnOnFirework()
    {
        FireworkEffect.Play();
        SoundManage.Instance.Play_Firework();
    }
    public void TurnOffFirework()
    {
        FireworkEffect.Stop();
    }
    public void TurnOnUpgradeHPEffect()
    {
        UpgradeHPEffect.Play();
    }
    public void TurnOnUpgradeDamageEffect()
    {
        UpgradeDamageEffect.Play();
    }

    public void TurnOnCoinEffect(Vector3 position)
    {

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            TurnOnBomb(Vector3.zero);
        }
    }
}
