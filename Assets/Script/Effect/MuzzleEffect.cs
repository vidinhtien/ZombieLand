using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffect : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> listParticle;
    public void Play()
    {
        for(int i=0; i<listParticle.Count; i++)
        {
            listParticle[i].Play();
        }
        Invoke("Stop", 2f);
    }
    public void Stop()
    {
        gameObject.SetActive(false);
        //for (int i = 0; i < listParticle.Count; i++)
        //{
        //    listParticle[i].Stop();
        //}
    }
    
}
