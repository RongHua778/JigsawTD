using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{

    public void ContinueClick()
    {
        this.gameObject.SetActive(false);
    }

    public void ExitClick()
    {
        if (Game.Instance != null)
            Game.Instance.QuitGame();
    }

}
