using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : Singleton<MenuUIManager>
{
    //界面系统
    [SerializeField] UILevelManager m_UILevelManager = default;
    [SerializeField] MenuMessage m_Message = default;

    public void Initinal()
    {
        Sound.Instance.PlayBg("menu");
        m_Message.Initialize();
        m_UILevelManager.Initialize();

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
            PlayerPrefs.DeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST2"));
            PlayerPrefs.SetInt("MaxPassLevel", 3);
        }
    }

    public void StartGameBtnClick()
    {
        m_UILevelManager.Show();
        m_UILevelManager.SetLevelInfo();
    }



    public void BluePrintBtnClick()
    {
        ShowMessage("暂未开放");
    }

    public void SettingBtnClick()
    {
        ShowMessage("暂未开放");
    }

    private void ShowMessage(string content)
    {
        m_Message.SetText(content);
    }


    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
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
