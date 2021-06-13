using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    public void NotBtnClick()
    {
        Game.Instance.Tutorial = false;
        Game.Instance.LoadScene(1);
    }

    public void YesBtnClick()
    {
        Game.Instance.Tutorial = true;
        Game.Instance.LoadScene(1);
    }
}
