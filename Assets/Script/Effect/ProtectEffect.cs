using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectEffect : BaseEffect
{
    [SerializeField]
    ParticleSystem startParticle;
    [SerializeField]
    ParticleSystem loopParticle;
    [SerializeField]
    ParticleSystem endParticle;
    [SerializeField]
    float timePlay;
    [SerializeField]
    float timeProtect;
    float _timeProtect;
    float timeStart;
    float _timeLoop;
    float timeEnd;
    float _timeStart;
    float _timeEnd;
    bool isProtect, startDone, loopDone, endDone;
    public bool IsProtecting { get { return isProtect; } }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        isProtect = false;
        timeStart = startParticle.main.startLifetime.constant;
        timeEnd = endParticle.main.startLifetime.constant;
    }
    public void Setup()
    {
        _timeStart = timeStart;
        _timeEnd = timeEnd;
        startDone = false;
        loopDone = false;
        endDone = false;
        _timeLoop = timePlay - timeStart - timeEnd;
    }
    public override void Play()
    {
        startParticle.Play();
    }
    public void Play(float time)
    {
        timePlay = time;
        Setup();
        startParticle.Play();
        isProtect = true;
    }
    public void PlayLoop()
    {
        
        loopParticle.Play();
    }
    public void PlayAgain(float time)
    {
        if (isProtect)
        {
            if(_timeLoop <= 0)
            {
                _timeLoop = time;
            }
            else
            {
                _timeLoop += time;
            }
            if (endDone)
            {
                endDone = false;
                if (!loopParticle.isPlaying && endParticle.isPlaying)
                {
                    loopParticle.Play();
                }
            }
            endParticle.Stop();
        }
        else
        {
            Play(time);
        }
    }
    public void End()
    {
        endParticle.Play();
    }
    public override void Stop()
    {
        startParticle.Stop();
        loopParticle.Stop();
        endParticle.Stop();

    }
    private void Update()
    {
        if (isProtect)
        {
            if(_timeStart > 0)
            {
                if (!startDone)
                {
                    startDone = true;
                    startParticle.Play();
                }
                _timeStart -= Time.deltaTime;
            }
            else
            {
                if (!loopDone)
                {
                    loopDone = true;
                    loopParticle.Play();
                }
                if(_timeLoop > 0)
                {
                    _timeLoop -= Time.deltaTime;
                }
                else
                {
                    if (!endDone)
                    {
                        endDone = true;
                        loopParticle.Stop();
                        endParticle.Play();
                    }
                    if (_timeEnd > 0)
                    {
                        _timeEnd -= Time.deltaTime;
                    }
                    else
                    {
                        isProtect = false;
                        Stop();
                    }
                }
            }
        }
    }
}
