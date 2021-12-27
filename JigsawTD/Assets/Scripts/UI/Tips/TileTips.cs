using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileTips : IUserInterface
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Image Icon = default;
    [SerializeField] protected TextMeshProUGUI Name = default;
    [SerializeField] protected TextMeshProUGUI Description = default;


    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
    }


    //public override void Hide()
    //{
    //    anim.SetBool("isOpen", false);
    //}

    public virtual void CloseTips()
    {
        anim.SetBool("isOpen", false);
    }
}
