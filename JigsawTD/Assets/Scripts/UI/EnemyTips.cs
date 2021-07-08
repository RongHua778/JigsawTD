using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTips : IUserInterface
{
    [SerializeField] EnemyGrid[] enemyGrids = default;


    public void ReadSequenceInfo(WaveSystem waveSystem)
    {
        EnemySequence sequence = waveSystem.RunningSequence[0];
        for(int i = 0; i < 3; i++)
        {
            if (i >= sequence.EnemyAttribute.Count)
            {
                enemyGrids[i].gameObject.SetActive(false);
            }
            else
            {
                enemyGrids[i].gameObject.SetActive(true);
                enemyGrids[i].SetEnemyInfo(sequence.EnemyAttribute[i]);
            }
        }
    }


}
