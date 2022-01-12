using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class MainUI : IUserInterface
{

    static Animator CoinAnim, WaveAnim, LifeAnim;

    //UI
    [SerializeField] Image GameSpeedImg = default;
    [SerializeField] Text PlayerLifeTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] WaveInfoSetter m_WaveInfoSetter = default;
    [SerializeField] Text waveTxt = default;

    public int CurrentWave
    {
        set
        {
            string lang = PlayerPrefs.GetString("_language");
            switch (lang)
            {
                case "ch":
                    waveTxt.text = GameMultiLang.GetTraduction("NUM") + value + (LevelManager.Instance.CurrentLevel.Difficulty == 99 ? "" : "/" + LevelManager.Instance.CurrentLevel.Wave) + GameMultiLang.GetTraduction("WAVE");
                    break;
                case "en":
                    waveTxt.text = GameMultiLang.GetTraduction("WAVE") + value + (LevelManager.Instance.CurrentLevel.Difficulty == 99 ? "" : "/" + LevelManager.Instance.CurrentLevel.Wave);
                    break;
            }
        }
    }

    public int Coin
    {
        set => coinTxt.text = value.ToString();
    }

    public int Life
    {
        set => PlayerLifeTxt.text = value.ToString() + "/" + LevelManager.Instance.CurrentLevel.PlayerHealth.ToString();
    }



    [SerializeField] Sprite[] GameSpeedSprites = default;
    //ÓÎÏ·ËÙ¶È
    private int gameSpeed = 1;
    public int GameSpeed
    {
        get => gameSpeed;
        set
        {
            if (value > 3)
            {
                gameSpeed = 1;
            }
            else
            {
                gameSpeed = value;
            }
            GameSpeedImg.sprite = GameSpeedSprites[GameSpeed - 1];
            Time.timeScale = gameSpeed;
        }
    }


    public override void Initialize()
    {
        base.Initialize();
        GameSpeed = 1;
        CoinAnim = m_RootUI.transform.Find("Coin").GetComponent<Animator>();
        LifeAnim = m_RootUI.transform.Find("Life").GetComponent<Animator>();
        WaveAnim = m_RootUI.transform.Find("Wave").GetComponent<Animator>();

    }



    public override void Release()
    {
        base.Release();
        GameSpeed = 1;
    }


    public static void PlayMainUIAnim(int part, string key, bool value)
    {
        switch (part)
        {
            case 0://Coin
                CoinAnim.SetBool(key, value);
                break;
            case 1://Life
                LifeAnim.SetBool(key, value);
                break;
            case 2://Wave
                WaveAnim.SetBool(key, value);
                break;
        }
    }

    public override void Show()
    {
        PlayMainUIAnim(0, "Show", true);
        PlayMainUIAnim(1, "Show", true);
        PlayMainUIAnim(2, "Show", true);
    }

    public override void Hide()
    {
        PlayMainUIAnim(0, "Show", false);
        PlayMainUIAnim(1, "Show", false);
        PlayMainUIAnim(2, "Show", false);
    }


    public void PrepareNextWave(List<EnemySequence> sequences,EnemyType nextBoss)
    {
        m_WaveInfoSetter.SetWaveInfo(sequences,nextBoss);
    }

    public bool ConsumeMoney(int cost)
    {
        if (GameRes.Coin >= cost)
        {
            GameRes.Coin -= cost;
            return true;
        }
        else
        {
            GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKMONEY"));
            return false;
        }
    }

    public void GuideBookBtnClick()
    {
        GameManager.Instance.ShowGuideVideo(0);
    }

    public void GameSpeedBtnClick()
    {
        GameSpeed++;
    }


}
