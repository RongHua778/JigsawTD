using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : Singleton<Sound>
{

    AudioSource m_bgSound;
    AudioSource m_effectSound;
    public string ResourceDir = "";

    protected override void Awake()
    {
        base.Awake();
        m_bgSound = this.gameObject.AddComponent<AudioSource>();
        m_bgSound.playOnAwake = false;
        m_bgSound.loop = true;

        m_effectSound = this.gameObject.AddComponent<AudioSource>();
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
        if (m_bgSound.clip != null)
        {
            string path;
            if (string.IsNullOrEmpty(ResourceDir))
                path = "";
            else
                path = ResourceDir + "/" + audioName;
            //加载音乐文件
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                StopAllCoroutines();
                StartCoroutine(FadeMusic(clip));
            }
            return;
        }
        //当前正在播放的音乐文件
        string oldName;
        if (m_bgSound.clip == null)
        {
            oldName = null;
        }
        else
        {
            oldName = m_bgSound.clip.name;
        }

        if (oldName != audioName)
        {
            //音乐文件路径
            string path;
            if (string.IsNullOrEmpty(ResourceDir))
                path = "";
            else
                path = ResourceDir + "/" + audioName;
            //加载音乐文件
            AudioClip clip = Resources.Load<AudioClip>(path);

            //播放
            if (clip != null)
            {
                m_bgSound.clip = clip;
                m_bgSound.Play();
            }
        }
    }

    private IEnumerator FadeMusic(AudioClip clip)
    {
        float startVolume = m_bgSound.volume;

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
        //路径
        string path;
        if (string.IsNullOrEmpty(ResourceDir))
            path = "";
        else
            path = ResourceDir + "/" + audioName;

        //音频
        AudioClip clip = Resources.Load<AudioClip>(path);
        //播放
        m_effectSound.PlayOneShot(clip);

    }

    public void PlayEffect(AudioClip clip, float volume = 1)
    {
        m_effectSound.volume = volume;
        m_effectSound.PlayOneShot(clip,0.5f);
    }
}
