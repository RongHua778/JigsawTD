using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

public class GameEndUI : IUserInterface
{
    [SerializeField] TurretBillboard m_BillBoard = default;
    [SerializeField] Image titleBG = default;
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] Text passTimeTxt = default;
    [SerializeField] Text totalCompositeTxt = default;
    [SerializeField] Text totalDamageTxt = default;
    [SerializeField] Text maxPathTxt = default;
    [SerializeField] Text maxMarkTxt = default;
    [SerializeField] Text gainGoldTxt = default;
    [SerializeField] Text expValueTxt = default;
    //[SerializeField] InfoBtn expInfoBtn = default;
    [SerializeField] GameLevelHolder gameLevelPrefab = default;

    [SerializeField] GameObject NextLevelBtn = default;
    [SerializeField] GameObject RestartBtn = default;

    [Header("标题底图")]
    [SerializeField] Sprite[] TitleBGs = default;

    int changeSpeed = 10;
    float waittime = 0.05f;
    float result = 0;
    Animator anim;
    int gainExp = 0;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }
    public override void Initialize()
    {
        base.Initialize();
        gainExp = 0;
        gameLevelPrefab.SetData();
    }

    public void SetGameResult(bool win)
    {
        LevelManager.Instance.LevelEnd = true;
        gainExp = LevelManager.Instance.GainExp(GameRes.CurrentWave);

        //expInfoBtn.SetContent(GameMultiLang.GetTraduction("EXPVALUE") + "=" +
        //    GameMultiLang.GetTraduction("DIFFICULTY") + "*5*" +
        //    GameMultiLang.GetTraduction("WAVE") + "*(1+25%*" +
        //    GameMultiLang.GetTraduction("BOSSDEFEAT") + ")");

        if (LevelManager.Instance.CurrentLevel.Difficulty == 99)//无尽模式
        {
            title.text = GameMultiLang.GetTraduction("PASSLEVEL") + (GameRes.CurrentWave) + GameMultiLang.GetTraduction("WAVE");
            LevelManager.Instance.EndlessHighScore = GameRes.CurrentWave;
            int tempWordID = Mathf.Clamp(GameRes.CurrentWave / 20, 0, 5);
            titleBG.sprite = TitleBGs[tempWordID + 2];
            GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.EndlessEnd, tempWordID));
            SteamLeaderboard.UpdateScore(GameRes.CurrentWave);
            NextLevelBtn.SetActive(false);
            RestartBtn.SetActive(true);
        }
        else//标准模式
        {
            if (win)
            {
                title.text = GameMultiLang.GetTraduction("WIN") + GameMultiLang.GetTraduction("DIFFICULTY") + (LevelManager.Instance.CurrentLevel.Difficulty).ToString();
                GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.StandardWin, LevelManager.Instance.CurrentLevel.Difficulty));
                LevelManager.Instance.PassDiifcutly = LevelManager.Instance.CurrentLevel.Difficulty + 1;
                titleBG.sprite = TitleBGs[LevelManager.Instance.CurrentLevel.Difficulty + 1];
                if (LevelManager.Instance.CurrentLevel.Difficulty < 6)//当前难度低于最大难度时，显示下一难度
                {
                    NextLevelBtn.SetActive(true);
                    RestartBtn.SetActive(false);
                }
                else
                {
                    NextLevelBtn.SetActive(false);
                    RestartBtn.SetActive(false);
                }

            }
            else
            {
                title.text = GameMultiLang.GetTraduction("LOSE");
                GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.StandardLose, LevelManager.Instance.CurrentLevel.Difficulty));

                titleBG.sprite = TitleBGs[0];
                NextLevelBtn.SetActive(false);
                RestartBtn.SetActive(true);
            }
        }
        m_BillBoard.SetBillBoard();
        StartCoroutine(SetValueCor());

        //if (LevelManager.Instance.CurrentLevel.Mode > 1)//难度2开始统计数据
        //{
        //    ModeData data = SetModeData(LevelManager.Instance.CurrentLevel.Mode, GameRes.CurrentWave, win, m_BillBoard.HighestTurret);
        //    UnityAnalystics.UploadModeData(data);
        //}

    }

    private ModeData SetModeData(int modeID, int wave, bool win, TurretContent highestTurret)
    {
        ModeData modeData = new ModeData();
        modeData.ModeID = LevelManager.Instance.CurrentLevel.Mode;
        modeData.Wave = wave;
        //首次胜利数据
        if (win)
        {
            int winCount = PlayerPrefs.GetInt("Mode" + modeID + "Win", 0);
            winCount++;
            PlayerPrefs.SetInt("Mode" + modeID + "Win", winCount);
            if (winCount > 0 && winCount < 2)   //  首次取胜
            {
                modeData.BeforeLost = PlayerPrefs.GetInt("Mode" + modeID + "Lost", 0);
            }
            else
            {
                modeData.BeforeLost = 999;
            }
        }
        else
        {
            int lostCount = PlayerPrefs.GetInt("Mode" + modeID + "Lost", 0);
            lostCount++;
            PlayerPrefs.SetInt("Mode" + modeID + "Lost", lostCount);
        }

        if (highestTurret != null)
        {
            modeData.HighestTurretStrategy = highestTurret.Strategy;
        }
        return modeData;
    }


    private void SetExp()
    {
        expValueTxt.text = GameMultiLang.GetTraduction("EXPVALUE") + ":" + gainExp;
        gameLevelPrefab.AddExp(gainExp);
    }

    IEnumerator SetValueCor()
    {
        passTimeTxt.text = "";
        totalCompositeTxt.text = "";
        totalDamageTxt.text = "";
        maxPathTxt.text = "";
        maxMarkTxt.text = "";
        gainGoldTxt.text = "";
        expValueTxt.text = "";

        float delta;   //delta为速度，每次加的数大小
        yield return new WaitForSeconds(waittime);
        passTimeTxt.text = (GameRes.LevelStart - DateTime.Now).ToString(@"hh\:mm\:ss");

        delta = (float)GameRes.TotalRefactor / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            totalCompositeTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        totalCompositeTxt.text = GameRes.TotalRefactor.ToString();

        delta = (float)GameRes.TotalDamage / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            totalDamageTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        totalDamageTxt.text = GameRes.TotalDamage.ToString();

        delta = (float)GameRes.MaxPath / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            maxPathTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        maxPathTxt.text = GameRes.MaxPath.ToString();

        delta = (float)GameRes.MaxMark / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            maxMarkTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        maxMarkTxt.text = GameRes.MaxMark.ToString();

        delta = (float)GameRes.GainGold / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            gainGoldTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        gainGoldTxt.text = GameRes.GainGold.ToString();

        SetExp();
        StopCoroutine(SetValueCor());
    }

    public void NextLevelBtnClick()
    {
        if (LevelManager.Instance.CurrentLevel.Difficulty + 1 > LevelManager.Instance.PermitDifficulty)
        {
            TempWord tempWord = new TempWord(TempWordType.Demo, 0);
            GameEvents.Instance.TempWordTrigger(tempWord);
            return;
        }
        LevelManager.Instance.StartNewGame(LevelManager.Instance.CurrentLevel.Difficulty + 1);
    }


    public override void Show()
    {
        base.Show();
        anim.SetBool("Show", true);
    }




}
