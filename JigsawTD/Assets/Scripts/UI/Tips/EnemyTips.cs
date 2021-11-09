using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTips : IUserInterface
{
    [SerializeField] EnemyGrid[] enemyGrids = default;


    public void ReadSequenceInfo(List<EnemySequence> sequences)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i >= sequences.Count)
            {
                enemyGrids[i].gameObject.SetActive(false);
            }
            else
            {
                enemyGrids[i].gameObject.SetActive(true);
                enemyGrids[i].SetEnemyInfo(StaticData.Instance.EnemyFactory.Get(sequences[i].EnemyType));
            }
        }
    }


}
