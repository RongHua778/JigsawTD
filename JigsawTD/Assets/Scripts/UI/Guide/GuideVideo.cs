using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideVideo :IUserInterface
{
    [SerializeField] Toggle[] tabs = default;
    Animator anim;

    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
        ShowPage(0);
        
    }

    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
    }

    public void ShowPage(int index)
    {
        tabs[index].isOn = true;
    }

    public override void Hide()
    {
        anim.SetBool("isOpen", false);
        GameEvents.Instance.TutorialTrigger(TutorialType.GuideBookContinue);
    }

}
