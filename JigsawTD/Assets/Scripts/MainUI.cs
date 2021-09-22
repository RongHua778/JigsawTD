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
    [SerializeField] GuideBook m_GuideBook = default;



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
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameSpeed = 1;

        m_PausePanel.Initialize();
        m_GuideBook.Initialize();
        m_Anim = GetComponent<Animator>();

    }



    public override void Release()
    {
        base.Release();
        GameSpeed = 1;
        GameEvents.Instance.onEnemyReach -= EnemyReach;
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

    private void EnemyReach(Enemy enemy)
    {
        GameRes.Life -= enemy.ReachDamage;
    }



    public void PrepareNextWave(List<EnemySequence> sequences)
    {
        GameRes.CurrentWave++;
        GameManager.Instance.GainInterest();
        GameManager.Instance.GainMoney(Mathf.Min(300, (StaticData.Instance.BaseWaveIncome +
            StaticData.Instance.WaveMultiplyIncome * (GameRes.CurrentWave - 1))));
        m_WaveInfoSetter.SetWaveInfo(GameRes.CurrentWave, sequences);
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
