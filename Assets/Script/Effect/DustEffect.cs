using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> listParticle;
    [SerializeField]
    float timeDeactive = 2f;
    public void Play()
    {
        for (int i = 0; i < listParticle.Count; i++)
        {
            listParticle[i].Play();
        }
        Invoke("Stop", timeDeactive);
    }
    public void Stop()
    {
        gameObject.SetActive(false);
    }
}
