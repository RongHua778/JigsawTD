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
    [SerializeField] BonusSetter bonusSetter = default;
    [SerializeField] Toggle tutorialCheck = default;
    [SerializeField] CanvasGroup startGameBtnSprite = default;

    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
        bonusSetter.SetInfo();
        tutorialCheck.isOn = false;
    }
    public void SetLevelInfo()
    {
        DifficultyBtnClick(0);
    }

    public void StartGameBtnClick()
    {
        if (!gameStart)
        {
            if (LevelManager.Instance.SelectedLevelID > LevelManager.Instance.PremitDifficulty)
            {
                MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("UNPERMIT"));
                return;
            }
            else if (LevelManager.Instance.SelectedLevelID > LevelManager.Instance.PassDiifcutly)
            {
                MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("UNPASS"));
                return;
            }
            gameStart = true;
            Game.Instance.LoadScene(1);
        }
    }

    public void DifficultyBtnClick(int count)
    {
        LevelManager.Instance.SelectedLevelID += count;
        difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + LevelManager.Instance.SelectedLevelID);
        if (LevelManager.Instance.SelectedLevelID == 0)
        {
            difficultyTxt.text = GameMultiLang.GetTraduction("TUTORIAL");
            tutorialCheck.gameObject.SetActive(true);
        }
        else
        {
            difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + LevelManager.Instance.SelectedLevelID.ToString();
            tutorialCheck.gameObject.SetActive(false);
        }
        if (LevelManager.Instance.SelectedLevelID > LevelManager.Instance.PremitDifficulty)
        {
            startGameBtnSprite.alpha = 0.5f;
        }
        else
        {
            startGameBtnSprite.alpha = 1f;
        }

        bonusSetter.SetInfo();
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
