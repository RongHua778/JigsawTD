using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : IUserInterface
{

    public void RestartBtnClick()
    {
        Game.Instance.LoadScene(1);
    }

    public void ExitClick()
    {
        Game.Instance.QuitGame();
    }

    public void ReturnToMenu()
    {
        Game.Instance.LoadScene(0);
    }

}
