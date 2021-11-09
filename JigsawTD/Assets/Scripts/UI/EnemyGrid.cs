using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGrid : MonoBehaviour
{
    [SerializeField] Image enemyIcon = default;
    [SerializeField] Text enemyName = default;
    [SerializeField] Text enemyDes = default;
    [SerializeField] GameObject bossIcon = default;

    [SerializeField] EnemyGrid_Attbar[] attBars = default;

    public void SetEnemyInfo(EnemyAttribute attribute)
    {
        enemyIcon.sprite = attribute.Icon;
        enemyName.text = GameMultiLang.GetTraduction(attribute.Name);
        enemyDes.text = GameMultiLang.GetTraduction(attribute.Description);
        attBars[0].SetAtt(attribute.HealthAtt);
        attBars[1].SetAtt(attribute.SpeedAtt);
        attBars[2].SetAtt(attribute.AmountAtt);
        attBars[3].SetAtt(attribute.ReachDamage);

        bossIcon.SetActive(attribute.IsBoss);
    }


}
