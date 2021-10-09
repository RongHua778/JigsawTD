using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : IUserInterface
{
    [SerializeField] Text title = default;
    [SerializeField] Text totalCompositeTxt = default;
    [SerializeField] Text totalDamageTxt = default;
    Animator anim;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }
    //public void SetGameResult(int turn)
    //{
    //    title.text = GameMultiLang.GetTraduction("PASSLEVEL") + (turn - 1) + GameMultiLang.GetTraduction("WAVE");
    //    int maxLevel = LevelManager.Instance.LevelMaxTurn;
    //    if ((turn - 1) > maxLevel)
    //    {
    //        LevelManager.Instance.LevelMaxTurn = turn - 1;
    //    }
    //    levelHighScore.text = LevelManager.Instance.LevelMaxTurn.ToString();
    //    totalCompositeTxt.text = TotalComposite.ToString();
    //    totalDamageTxt.text = TotalDamage.ToString();
    //}

    public void SetGameResult(bool win)
    {
        title.text = win ? GameMultiLang.GetTraduction("WIN") : GameMultiLang.GetTraduction("LOSE");
        if (win)
            PlayerPrefs.SetInt("MaxDifficulty", LevelManager.Instance.SelectedLevelID + 1);
        totalCompositeTxt.text = GameRes.TotalRefactor.ToString();
        totalDamageTxt.text = GameRes.TotalDamage.ToString();
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
        anim.SetBool("isOpen", true);
    }

    public override void Hide()
    {
        anim.SetBool("isOpen", false);
    }



}
