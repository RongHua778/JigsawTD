using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMode : IUserInterface
{
    private Animator m_Anim;

    [SerializeField] UIStandardMode m_UIStandardMode = default;
    [SerializeField] UIEndlessMode m_UIEndlessMode = default;

    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();
        m_Anim.SetBool("OpenLevel", true);
        m_UIStandardMode.SetInfo();
        m_UIEndlessMode.SetInfo();
    }

    public override void ClosePanel()
    {
        m_Anim.SetBool("OpenLevel", false);
        MenuManager.Instance.ShowMenu();
    }

}
