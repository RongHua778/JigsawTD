using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : Singleton<Sound>
{

    private Dictionary<string, AudioClip> BGmusic;
    private Dictionary<string, AudioClip> EffectMusic;

    AudioSource m_bgSound;
    AudioSource m_effectSound;
    public string ResourceDir = "";

    public AudioClip SoilderClip;
    public AudioClip RestorerClip;
    public AudioClip TankerClip;
    public AudioClip RunnerClip;
    public AudioClip LastWaveClip;

    protected override void Awake()
    {
        base.Awake();
        InitializeMusicDIC();

        m_bgSound = this.gameObject.AddComponent<AudioSource>();
        m_bgSound.playOnAwake = false;
        m_bgSound.loop = true;

        m_effectSound = this.gameObject.AddComponent<AudioSource>();
    }

    private void InitializeMusicDIC()
    {
        BGmusic = new Dictionary<string, AudioClip>();
        EffectMusic = new Dictionary<string, AudioClip>();

        AudioClip[] bgs = Resources.LoadAll<AudioClip>(ResourceDir + "/BGs");
        foreach (var clip in bgs)
        {
            BGmusic.Add(clip.name, clip);
        }

        AudioClip[] effects = Resources.LoadAll<AudioClip>(ResourceDir+"/Effects");
        foreach(var clip in effects)
        {
            EffectMusic.Add(clip.name, clip);
        }
    }

    //音乐大小
    public float BgVolume
    {
        get { return m_bgSound.volume; }
        set { m_bgSound.volume = value; }
    }
    //音效大小
    public float EffectVolume
    {
        get { return m_effectSound.volume; }
        set { m_effectSound.volume = value; }
    }

    //播放音乐
    public void PlayBg(string audioName)
    {
        if (!BGmusic.ContainsKey(audioName))
        {
            Debug.Log("使用了错误的音乐名");
            return;
        }
        AudioClip clip = BGmusic[audioName];
        if (m_bgSound.clip != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeMusic(clip));
            return;
        }
        else
        {
            m_bgSound.clip = clip;
            m_bgSound.Play();
        }
    }

    private IEnumerator FadeMusic(AudioClip clip)
    {
        float startVolume = 0.5f;

        while (m_bgSound.volume > 0)
        {
            m_bgSound.volume -= startVolume * Time.deltaTime / 2f;
            yield return null;
        }
        m_bgSound.clip = clip;
        m_bgSound.Play();
        while (m_bgSound.volume <= startVolume)
        {
            m_bgSound.volume += startVolume * Time.deltaTime / 2f;
            yield return null;
        }

    }
    //停止音乐
    public void StopBg()
    {
        m_bgSound.Stop();
        m_bgSound.clip = null;
    }
    //播放音效
    public void PlayEffect(string audioName)
    {
        if (!EffectMusic.ContainsKey(audioName))
        {
            Debug.Log("使用了错误的音效名");
            return;
        }
        AudioClip clip = EffectMusic[audioName];
        m_effectSound.PlayOneShot(clip);


    }

    public void PlayEffect(AudioClip clip, float volume = 1)
    {
        m_effectSound.volume = volume;
        m_effectSound.PlayOneShot(clip,0.5f);
    }

}
