using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public static SoundManage Instance;
    public const string SOUNDKEY = "Sound";
    public const string MUSICKEY = "Music";
    public const string VIBRATEKEY = "Vibrate";
    bool _soundBool;
    bool _vibrateBool;
    bool _musicBool;
    public bool IsSoundOn { get { return _soundBool; } }
    public bool IsVibrateOn { get { return _vibrateBool; } }
    public bool IsMusicOn { get { return _musicBool; } }
    [SerializeField]
    AudioSource sound_audioSource;
    [SerializeField]
    AudioSource music_audioSource;
    [SerializeField]
    AudioClip[] shootsAudios;
    [SerializeField]
    AudioClip[] bgHomes;
    [SerializeField]
    AudioClip[] bgGames;
    [SerializeField]
    AudioClip buttonclick;
    [SerializeField]
    AudioClip buttonclickOpen;
    [SerializeField]
    AudioClip buttonclickClose;
    [SerializeField]
    AudioClip playClick;
    [SerializeField]
    AudioClip[] footStepSingle;
    [SerializeField]
    AudioClip fireworkClip;
    [SerializeField]
    AudioClip coinPickUpClip;
    [SerializeField]
    AudioClip coinGainClip;
    [SerializeField]
    AudioClip reviveClip;
    [SerializeField]
    AudioClip upgradeClip;
    [SerializeField]
    AudioClip[] zombieGroanClips;
    [SerializeField]
    AudioClip[] zombieDeadClips;
    [SerializeField]
    float maxSoundVolume = 0.7f;
    [SerializeField]
    float maxMusicVolume = 0.2f;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        FetchData();
        sound_audioSource.volume = maxSoundVolume;
    }
    [SerializeField]
    float delayShootSound = 0.4f;
    float _delayShootSound;
    [SerializeField]
    float delayfootStepSound = 0.12f;
    float _delayfootStepSound;
    [SerializeField]
    float delayZombieGroanSound = 0.12f;
    float _delayZombieGroanSound = 0;
    [SerializeField]
    float delayZombieDeadSound = 0.12f;
    float _delayZombieDeadSound = 0;
    private void Update()
    {
        if (_delayShootSound > 0)
        {
            _delayShootSound -= Time.deltaTime;
        }
        if (_delayfootStepSound > 0)
        {
            _delayfootStepSound -= Time.deltaTime;
        }
        if (_delayCoinSound > 0)
        {
            _delayCoinSound -= Time.deltaTime;
        }
        if (_delayZombieGroanSound > 0)
        {
            _delayZombieGroanSound -= Time.deltaTime;
        }
        if (_delayZombieDeadSound > 0)
        {
            _delayZombieDeadSound -= Time.deltaTime;
        }
    }

    public static void FirstInit()
    {
        PlayerPrefs.SetInt(SOUNDKEY, 1);
        PlayerPrefs.SetInt(MUSICKEY, 1);
        PlayerPrefs.SetInt(VIBRATEKEY, 1);
    }

    void FetchData()
    {
        _soundBool = PlayerPrefs.GetInt(SOUNDKEY, 1) == 1;
        _vibrateBool = PlayerPrefs.GetInt(VIBRATEKEY, 1) == 1;
        _musicBool = PlayerPrefs.GetInt(MUSICKEY, 1) == 1;

    }

    public void SetSoundActive(bool set)
    {
        if (set)
        {
            PlayerPrefs.SetInt(SOUNDKEY, 1);
            _soundBool = true;
        }
        else
        {
            PlayerPrefs.SetInt(SOUNDKEY, 0);
            _soundBool = false;
        }
    }
    public void SetMusicActive(bool set)
    {
        if (set)
        {
            PlayerPrefs.SetInt(MUSICKEY, 1);
            _musicBool = true;
            Play_HomeMusic();
        }
        else
        {
            PlayerPrefs.SetInt(MUSICKEY, 0);
            _musicBool = false;
            StopMusic();
        }
    }

    public void SetVibrateActive(bool set)
    {
        if (set)
        {
            PlayerPrefs.SetInt(VIBRATEKEY, 1);
            _vibrateBool = true;
        }
        else
        {
            PlayerPrefs.SetInt(VIBRATEKEY, 0);
            _vibrateBool = false;
        }
    }

    public void Play_GunSound()
    {
        if (_soundBool)
        {
            if (_delayShootSound > 0) return;
            _delayShootSound = delayShootSound;
            int type = Random.Range(0, shootsAudios.Length);
            sound_audioSource.PlayOneShot(shootsAudios[type]);

        }
    }
    public void Play_ButtonClick()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(buttonclick);

    }
    public void Play_Revive()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(reviveClip);

    }
    public void Play_CoinGain()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(coinGainClip);

    }
    public void Play_ClickOpen()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(buttonclickClose);

    }
    public void Play_ClickClose()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(buttonclickClose);

    }
    public void Play_Upgrade()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(upgradeClip);

    }
    public void Play_PlayButtonClick()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(playClick);
    }
    public void Play_Firework()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(fireworkClip);
    }
    public void Play_CoinPickUp()
    {
        if (!_soundBool) return;
        sound_audioSource.PlayOneShot(coinPickUpClip);
    }
    [SerializeField]
    float delayCoinSound = 0.12f;
    float _delayCoinSound = 0;
    public void Play_CoinPickUpDelay()
    {
        if (!_soundBool || _delayCoinSound > 0) return;
        _delayCoinSound = delayCoinSound;
        sound_audioSource.PlayOneShot(coinPickUpClip);
    }
    public void Play_FootStep()
    {
        if (!_soundBool || _delayfootStepSound > 0) return;
        _delayfootStepSound = delayfootStepSound;
        sound_audioSource.PlayOneShot(footStepSingle[Random.Range((int)0, (int)footStepSingle.Length)]);
    }
    public void Play_ZombieScream()
    {
        if (!_soundBool) return;
        if (_delayZombieGroanSound > 0) return;
        _delayZombieGroanSound = delayZombieGroanSound;
        sound_audioSource.PlayOneShot(zombieGroanClips[Random.Range((int)0, (int)zombieGroanClips.Length)]);
    }
    public void Play_ZombieDead()
    {
        if (!_soundBool) return;
        if (_delayZombieDeadSound > 0) return;
        _delayZombieDeadSound = delayZombieDeadSound;
        sound_audioSource.PlayOneShot(zombieDeadClips[Random.Range((int)0, (int)zombieDeadClips.Length)]);
    }
    public void Play_HomeMusic()
    {
        if (!_musicBool) return;
        if (bgHomes.Length > 1)
        {
            music_audioSource.Stop();
            int rand = Random.Range(0, bgHomes.Length);
            music_audioSource.loop = true;
            OnChangeMusic(0.5f, bgHomes[rand]);
        }
        else if (bgHomes.Length == 1)
        {
            music_audioSource.Stop();
            music_audioSource.loop = true;
            OnChangeMusic(0.5f, bgHomes[0]);
        }
    }
    public void Play_GameMusic()
    {
        if (!_musicBool) return;
        if (bgGames.Length > 1)
        {
            int rand = Random.Range(0, bgGames.Length);
            music_audioSource.Stop();
            music_audioSource.loop = true;
            OnChangeMusic(0.5f, bgGames[rand]);
        }
        else if (bgGames.Length == 1)
        {
            music_audioSource.Stop();
            music_audioSource.loop = true;
            OnChangeMusic(0.5f, bgGames[0]);
        }
    }
    private void OnChangeMusic(float time, AudioClip clip)
    {
        StopAllCoroutines();
        StartCoroutine(OnChangeMusicIE(time, clip));
    }
    private IEnumerator OnChangeMusicIE(float time, AudioClip clip)
    {
        float tmp = music_audioSource.volume;
        float t = 0;
        while (t < time / 2)
        {
            t += Time.deltaTime;
            tmp = Mathf.Lerp(tmp, 0, t / (time / 2));
            music_audioSource.volume = tmp;
            yield return new WaitForEndOfFrame();
        }
        music_audioSource.clip = clip;
        music_audioSource.Play();
        while (t < time)
        {
            t += Time.deltaTime;
            tmp = Mathf.Lerp(tmp, maxMusicVolume, t / (time));
            music_audioSource.volume = tmp;
            yield return new WaitForEndOfFrame();
        }
    }

    public void FootstepOn()
    {

    }

    public void StopMusic()
    {
        music_audioSource.Stop();
    }
}
