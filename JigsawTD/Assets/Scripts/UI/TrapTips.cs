using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTips : TileTips
{

    public void ReadAttribute(TrapTile trapTile)
    {
        anim.SetBool("isOpen", true);
        Sound.Instance.PlayEffect("Sound_Click");

        Icon.sprite = trapTile.m_TrapAttribute.TrapIcon;
        Name.text = trapTile.m_TrapAttribute.Name;
        Description.text = trapTile.m_TrapAttribute.Description;
    }

}
