using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlot : MonoBehaviour
{
    [SerializeField] Image bossIcon = default;
    [SerializeField] Text turnTxt = default;

    public void SetBossInfo(EnemyAttribute attribute, int pass, int turn)
    {
        bossIcon.sprite = pass > turn ? attribute.EnemyIcon : attribute.EnemyEmptyIcon;
        turnTxt.text = GameMultiLang.GetTraduction("NUM") + turn + GameMultiLang.GetTraduction("WAVE");
    }
}
