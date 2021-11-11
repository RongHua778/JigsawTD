using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : IUserInterface
{
    [SerializeField] TurretBillboard m_BillBoard = default;
    [SerializeField] Text title = default;
    [SerializeField] Text totalCompositeTxt = default;
    [SerializeField] Text totalDamageTxt = default;
    [SerializeField] Text maxPathTxt = default;
    [SerializeField] Text maxMarkTxt = default;
    [SerializeField] Text gainGoldTxt = default;
    [SerializeField] Text expValueTxt = default;
    [SerializeField] InfoBtn expInfoBtn = default;
    [SerializeField] GameLevelHolder gameLevelPrefab = default;
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
        gainExp = LevelManager.Instance.GainExp(GameRes.CurrentWave);
        expInfoBtn.SetContent(GameMultiLang.GetTraduction("EXPVALUE") + "=" +
            GameMultiLang.GetTraduction("DIFFICULTY") + "*5*" +
            GameMultiLang.GetTraduction("WAVE") + "*(1+25%*" +
            GameMultiLang.GetTraduction("BOSSDEFEAT") + ")");

        if (LevelManager.Instance.CurrentLevel.IsEndless)
        {
            title.text = GameMultiLang.GetTraduction("WIN") + GameRes.CurrentWave + GameMultiLang.GetTraduction("WAVE");
            LevelManager.Instance.EndlessHighScore = GameRes.CurrentWave;
        }
        else
        {
            title.text = win ?
            GameMultiLang.GetTraduction("WIN") + GameMultiLang.GetTraduction("DIFFICULTY") + (LevelManager.Instance.SelectedLevelID + 1).ToString()
            : GameMultiLang.GetTraduction("LOSE");
        }

        if (win && !LevelManager.Instance.CurrentLevel.IsEndless)
        {
            LevelManager.Instance.PassDiifcutly = LevelManager.Instance.SelectedLevelID + 1;
        }
        m_BillBoard.SetBillBoard();
        StartCoroutine(SetValueCor());
    }

    private void SetExp()
    {
        expValueTxt.text = GameMultiLang.GetTraduction("EXPVALUE") + ":" + gainExp;
        gameLevelPrefab.AddExp(gainExp);
    }

    IEnumerator SetValueCor()
    {
        totalCompositeTxt.text = "";
        totalDamageTxt.text = "";
        maxPathTxt.text = "";
        maxMarkTxt.text = "";
        gainGoldTxt.text = "";
        expValueTxt.text = "";

        float delta;   //delta为速度，每次加的数大小
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

    public void ReturnToMenu()
    {
        if (Game.Instance != null)
        {
            Game.Instance.LoadScene(0);
        }
    }
    public override void Show()
    {
        base.Show();
        anim.SetBool("Show", true);
    }


    public void RestartBtnClick()
    {
        Game.Instance.LoadScene(1);
    }

}
