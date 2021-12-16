using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileTips : IUserInterface
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Image Icon = default;
    [SerializeField] protected Text Name = default;
    [SerializeField] protected TextMeshProUGUI Description = default;


    public override void Show()
    {
        //Sound.Instance.PlayEffect("Sound_Click");
        anim.SetBool("isOpen", true);
    }


    public override void Hide()
    {
        anim.SetBool("isOpen", false);
    }

}
