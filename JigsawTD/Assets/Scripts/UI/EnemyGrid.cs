using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGrid : MonoBehaviour
{
    [SerializeField] Image enemyIcon = default;
    [SerializeField] Text enemyName = default;
    [SerializeField] Text enemyDes = default;

    public void SetEnemyInfo(EnemySequence sequence)
    {
        EnemyAttribute attribute = GameManager.Instance.EnemyFactory.Get(sequence.EnemyType);
        enemyIcon.sprite = attribute.EnemyIcon;
        enemyName.text = attribute.EnemyName;
        enemyDes.text = attribute.Description;
    }


}
