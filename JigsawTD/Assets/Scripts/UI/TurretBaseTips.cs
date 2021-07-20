using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseTips : TileTips
{
    public void ReadTurretBase(TurretBaseContent trapContent)
    {
        Icon.sprite = trapContent.m_TurretBaseAttribute.Icon;
        Name.text = GameMultiLang.GetTraduction(trapContent.m_TurretBaseAttribute.Name);
        Description.text = GameMultiLang.GetTraduction(trapContent.m_TurretBaseAttribute.Description);
    }


}
