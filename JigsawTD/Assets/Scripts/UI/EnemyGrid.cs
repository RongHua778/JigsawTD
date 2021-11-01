using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGrid : MonoBehaviour
{
    [SerializeField] Image enemyIcon = default;
    [SerializeField] Text enemyName = default;
    [SerializeField] Text enemyDes = default;
    [SerializeField] Text enemyDamage = default;

    public void SetEnemyInfo(EnemySequence sequence)
    {
        EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(sequence.EnemyType);
        enemyIcon.sprite = attribute.Icon;
        enemyName.text = GameMultiLang.GetTraduction(attribute.Name);
        enemyDes.text = GameMultiLang.GetTraduction(attribute.Description);
        enemyDamage.text = GameMultiLang.GetTraduction("ENEMYDAMAGE") + ":" + attribute.ReachDamage;
    }


}
