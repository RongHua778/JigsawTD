using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideVideo :IUserInterface
{
    [SerializeField] Toggle[] tabs = default;
    Animator anim;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        anim = this.GetComponent<Animator>();
        if (Game.Instance.Tutorial)
        {
            foreach(var obj in tabs)
            {
                obj.gameObject.SetActive(false);
            }
            tabs[0].gameObject.SetActive(true);
        }
    }
    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
    }

    public void ShowPage(int index)
    {
        tabs[index].gameObject.SetActive(true);
        tabs[index].isOn = true;
    }

    public override void Hide()
    {
        anim.SetBool("isOpen", false);
        GameManager.Instance.TriggerGuide(10);
    }

}
