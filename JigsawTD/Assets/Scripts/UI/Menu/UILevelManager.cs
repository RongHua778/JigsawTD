using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : IUserInterface
{
    private bool gameStart = false;
    [SerializeField] UIBattleSet m_UIBattleSet = default;
    [SerializeField] Text difficultyInfo_Txt = default;
    private Animator m_Anim;
    [SerializeField] Text difficultyTxt = default;
    [SerializeField] Toggle tutorialCheck = default;
    [SerializeField] Text endlessHighScore = default;
    [SerializeField] Text endlessUnlockText = default;
    [SerializeField] GameObject endlessStartBtnObj = default;


    public override void Initialize()
    {
        base.Initialize();
        LevelManager.Instance.SelectDiffculty = LevelManager.Instance.PassDiifcutly;
        m_Anim = this.GetComponent<Animator>();
        tutorialCheck.isOn = false;
        endlessHighScore.text = LevelManager.Instance.EndlessHighScore + GameMultiLang.GetTraduction("WAVE");
    }
    public void SetLevelInfo()
    {
        DifficultyBtnClick(0);
        //是否已解锁无尽模式
        endlessStartBtnObj.SetActive(LevelManager.Instance.PassDiifcutly > 4);
        endlessUnlockText.gameObject.SetActive(LevelManager.Instance.PassDiifcutly <= 4);
    }

    public void StandardModeStart()
    {
        if (!gameStart)
        {
            LevelManager.Instance.HasLastGame = false;
            LevelManager.Instance.CurrentLevel = LevelManager.Instance.GetLevelAtt(LevelManager.Instance.SelectDiffculty);
            if (LevelManager.Instance.SelectDiffculty >= 1)
                Game.Instance.Tutorial = false;
            gameStart = true;
            Game.Instance.LoadScene(1);
        }
    }

    public void EndlessModeStart()
    {
        if (!gameStart)
        {
            LevelManager.Instance.HasLastGame = false;
            LevelManager.Instance.CurrentLevel = LevelManager.Instance.GetLevelAtt(11);
            gameStart = true;
            Game.Instance.LoadScene(1);
        }
    }



    public void DifficultyBtnClick(int count)
    {
        LevelManager.Instance.SelectDiffculty += count;
        difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + LevelManager.Instance.SelectDiffculty);
        if (LevelManager.Instance.SelectDiffculty == 0)//设置教程显示
        {
            difficultyTxt.text = GameMultiLang.GetTraduction("TUTORIAL");
            tutorialCheck.gameObject.SetActive(true);
            tutorialCheck.isOn = false;
        }
        else
        {
            difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + LevelManager.Instance.SelectDiffculty.ToString();
            tutorialCheck.isOn = true;
            tutorialCheck.gameObject.SetActive(false);
        }
    }

    public override void Show()
    {
        base.Show();
        m_Anim.SetBool("OpenLevel", true);
    }

    public void ClosePanel()
    {
        m_Anim.SetBool("OpenLevel", false);
    }

    public void OnBattleSetBtnClick()
    {
        m_UIBattleSet.Show();
    }

    public void OnTutorialCheck(bool value)
    {
        Game.Instance.Tutorial = !value;
    }
}
