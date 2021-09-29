using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : Singleton<MenuUIManager>
{
    //界面系统
    [SerializeField] UILevelManager m_UILevelManager = default;
    [SerializeField] MenuMessage m_Message = default;
    [SerializeField] UIBattleSet m_UIBattleSet = default;
    [SerializeField] TurretTips m_TurretTips = default;

    public void Initinal()
    {
        Sound.Instance.PlayBg("menu");
        m_Message.Initialize();
        m_UILevelManager.Initialize();
        m_UIBattleSet.Initialize();
        m_TurretTips.Initialize();

    }
    public void Release()
    {
        m_Message.Release();
    }

    public void GameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST1"));
            //   LevelManager.Instance.MaxDifficulty = 0;
            PlayerPrefs.DeleteAll();
        }
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    ShowMessage(GameMultiLang.GetTraduction("TEST2"));
        //    LevelManager.Instance.MaxDifficulty = 4;
        //    PlayerPrefs.SetInt("MaxDifficulty", 4);
        //}
    }

    public void StartGameBtnClick()
    {
        m_UILevelManager.Show();
        m_UILevelManager.SetLevelInfo();
    }

    public void ShowTurretTips(TurretAttribute att, Vector2 pos)
    {
        m_TurretTips.GetComponent<RectTransform>().anchoredPosition = pos;
        m_TurretTips.Show();
        m_TurretTips.ReadAttribute(att);
    }

    public void BluePrintBtnClick()
    {
        ShowMessage("暂未开放");
    }

    public void SettingBtnClick()
    {
        ShowMessage("暂未开放");
    }

    internal void HideTips()
    {
        m_TurretTips.Hide();
    }

    public void ShowMessage(string content)
    {
        m_Message.SetText(content);
    }


    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }

    public void UnlockAll()
    {

    }

    public void AddtoWishList()
    {
        Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");
    }

    //public void DifficultBtnClick(int value)
    //{
    //    Game.Instance.Difficulty += value;
    //    if (Game.Instance.Difficulty > Game.Instance.MaxDifficulty)
    //    {
    //        Game.Instance.Difficulty = 1;
    //    }
    //    else if (Game.Instance.Difficulty < 1)
    //    {
    //        Game.Instance.Difficulty = Game.Instance.MaxDifficulty;
    //    }
    //}


}
