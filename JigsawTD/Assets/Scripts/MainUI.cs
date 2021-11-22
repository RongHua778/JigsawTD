using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : IUserInterface
{
    public GameObject LifeObj;
    public GameObject MoneyObj;
    public GameObject TopLeftArea = default;
    public GameObject WaveObj;
    private Animator m_Anim;
    //UI
    [SerializeField] Image GameSpeedImg = default;
    [SerializeField] Text PlayerLifeTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] WaveInfoSetter m_WaveInfoSetter = default;
    [SerializeField] PausePanel m_PausePanel = default;
    [SerializeField] Text waveTxt = default;

    public int CurrentWave
    {
        set
        {
            string lang = PlayerPrefs.GetString("_language");
            switch (lang)
            {
                case "ch":
                    waveTxt.text = GameMultiLang.GetTraduction("NUM") + value + (LevelManager.Instance.CurrentLevel.Mode > 10 ? "" : "/" + LevelManager.Instance.CurrentLevel.Wave) + GameMultiLang.GetTraduction("WAVE");
                    break;
                case "en":
                    waveTxt.text = GameMultiLang.GetTraduction("WAVE") + value + (LevelManager.Instance.CurrentLevel.Mode > 10 ? "" : "/" + LevelManager.Instance.CurrentLevel.Wave);
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
        GameSpeed = 1;
        m_PausePanel.Initialize();
        m_Anim = GetComponent<Animator>();

    }



    public override void Release()
    {
        base.Release();
        GameSpeed = 1;
    }

    public void PrepareForGuide()
    {
        TopLeftArea.SetActive(false);
        MoneyObj.SetActive(false);

        LifeObj.SetActive(false);
        WaveObj.SetActive(false);
    }

    public override void Show()
    {
        m_Anim.SetBool("Show", true);
    }

    public void PlayAnim(string key, bool value)
    {
        m_Anim.SetBool(key, value);

    }


    public void PrepareNextWave(List<EnemySequence> sequences)
    {
        m_WaveInfoSetter.SetWaveInfo(sequences);
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

    public void PauseBtnClick()
    {
        m_PausePanel.Show();
    }

    public void GameSpeedBtnClick()
    {
        GameSpeed++;
    }


}
