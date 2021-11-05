using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoTips : TileTips
{
    [SerializeField] Text reachDamage_Txt = default;
    [SerializeField] Text background_Txt = default;

    public void ReadEnemyAtt(EnemyAttribute att)
    {
        Icon.sprite = att.Icon;
        Name.text = GameMultiLang.GetTraduction(att.Name);
        reachDamage_Txt.text = GameMultiLang.GetTraduction("ENEMYDAMAGE") + ":" + att.ReachDamage;
        Description.text = GameMultiLang.GetTraduction(att.Description);
        background_Txt.text= GameMultiLang.GetTraduction(att.BackGround);
    }
}
