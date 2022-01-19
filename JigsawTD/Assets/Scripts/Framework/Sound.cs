using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : Singleton<Sound>
{
    [SerializeField] AudioMixerGroup musicMixer = default;
    [SerializeField] AudioMixerGroup effectMixer = default;
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
    private Dictionary<string, AudioClip> BGmusic;
    private Dictionary<string, AudioClip> EffectMusic;

    AudioSource m_bgSound;
    AudioSource m_effectSound;
    public string ResourceDir = "";


    protected override void Awake()
    {
        base.Awake();
        InitializeMusicDIC();

        m_bgSound = this.gameObject.AddComponent<AudioSource>();
        m_bgSound.outputAudioMixerGroup = musicMixer;
        m_bgSound.playOnAwake = false;
        m_bgSound.loop = true;

        m_effectSound = this.gameObject.AddComponent<AudioSource>();
        m_effectSound.outputAudioMixerGroup = effectMixer;
        EffectVolume = 0.5f;
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

        AudioClip[] effects = Resources.LoadAll<AudioClip>(ResourceDir + "/Effects");
        foreach (var clip in effects)
        {
            EffectMusic.Add(clip.name, clip);
        }
    }



    //播放音乐
    public void PlayBg(string audioName)
    {
        if (!BGmusic.ContainsKey(audioName))
        {
            Debug.Log("使用了错误的音乐名");
            return;
        }
        if (m_bgSound.clip != null && audioName == m_bgSound.clip.name)
        {
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
            m_bgSound.volume -= startVolume * Time.deltaTime / 0.5f;
            yield return null;
        }
        m_bgSound.clip = clip;
        m_bgSound.Play();
        while (m_bgSound.volume <= startVolume)
        {
            m_bgSound.volume += startVolume * Time.deltaTime / 0.5f;
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

        //if (canPlay)
        //{
        //    canPlay = false;
        //    if (!EffectMusic.ContainsKey(audioName))
        //    {
        //        Debug.Log("使用了错误的音效名");
        //        return;
        //    }
        //    AudioClip clip = EffectMusic[audioName];
        //    m_effectSound.PlayOneShot(clip);
        //    StartCoroutine(Reset());
        //}
        if (!EffectMusic.ContainsKey(audioName))
        {
            Debug.Log("使用了错误的音效名");
            return;
        }
        AudioClip clip = EffectMusic[audioName];
        m_effectSound.PlayOneShot(clip);
        //StartCoroutine(Reset());

    }

    //bool canPlay = true;

    //IEnumerator Reset()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    canPlay = true;
    //}

    public void PlayEffect(AudioClip clip)
    {
        //m_effectSound.volume = volume;
        m_effectSound.PlayOneShot(clip, 0.5f);
    }

}
