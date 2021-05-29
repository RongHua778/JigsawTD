using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTips : TileTips
{
    public void ReadAttribute(TrapTile trapTile)
    {
        Icon.sprite = trapTile.m_TrapAttribute.TrapIcon;
        Name.text = trapTile.m_TrapAttribute.Name;
        Description.text = trapTile.m_TrapAttribute.Description;
    }

    public void CloseTips()
    {
        this.gameObject.SetActive(false);
    }
}
