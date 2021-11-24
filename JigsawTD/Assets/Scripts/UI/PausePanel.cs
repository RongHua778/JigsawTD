using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PausePanel : IUserInterface
{
    [SerializeField] Text ReturnTxt = default;
    [SerializeField] Text QuitTxt = default;
    public override void Show()
    {
        SetContent();
        base.Show();
    }

    private void SetContent()
    {
        if (Game.Instance.Tutorial)
        {
            ReturnTxt.text = GameMultiLang.GetTraduction("RETURNTUTORIAL");
            QuitTxt.text = GameMultiLang.GetTraduction("QUIT");
        }
        else
        {
            ReturnTxt.text = GameMultiLang.GetTraduction("RETURNTOMENU");
            QuitTxt.text = GameMultiLang.GetTraduction("QUIT2");
        }
    }

}
