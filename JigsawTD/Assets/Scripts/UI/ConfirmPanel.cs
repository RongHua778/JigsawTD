using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmPanel : IUserInterface
{
    private Animator m_Anim;
    [SerializeField] private Text MainTxt = default;
    [SerializeField] private Text ConfirmBtnTxt = default;
    [SerializeField] private Text CancelBtnTxt = default;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
    }

    public void SetContent(string mainTxt, string confirmTxt, string cancelTxt, UnityAction callback)
    {
        MainTxt.text = mainTxt;
        ConfirmBtnTxt.text = confirmTxt;
        CancelBtnTxt.text = cancelTxt;

        ConfirmBtnTxt.GetComponent<Button>().onClick.AddListener(callback);
    }
    public override void Show()
    {
        base.Show();
        m_Anim.SetBool("Show", true);
    }


}
