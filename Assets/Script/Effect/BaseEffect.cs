using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> listParticle;
    public virtual void Play()
    {
        Stop();
        for (int i = 0; i < listParticle.Count; i++)
        {
            listParticle[i].Play();
        }
    }
    public virtual void Stop()
    {
        for (int i = 0; i < listParticle.Count; i++)
        {
            listParticle[i].Stop();
        }
    }
}
