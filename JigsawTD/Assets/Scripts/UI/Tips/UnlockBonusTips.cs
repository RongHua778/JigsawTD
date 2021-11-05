using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBonusTips : IUserInterface
{
    private Animator anim;
    [SerializeField] UnlockSlot[] unlockSlots = default;

    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
    }

    public override void Show()
    {
        anim.SetBool("isOpen", true);
    }

    public void SetBouns(GameLevelInfo info)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < info.UnlockItems.Length)
            {
                unlockSlots[i].gameObject.SetActive(true);
                unlockSlots[i].SetBonus(info.UnlockItems[i]);
            }
            else
            {
                unlockSlots[i].gameObject.SetActive(false);
            }
        }
    }


    public override void Hide()
    {
        anim.SetBool("isOpen", false);
    }
}
