using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : IUserInterface
{

    public void RestartBtnClick()
    {

    }

    public void ExitClick()
    {
        if (Game.Instance != null)
            Game.Instance.QuitGame();
    }

}
