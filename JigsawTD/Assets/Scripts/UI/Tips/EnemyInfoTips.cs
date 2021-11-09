using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoTips : TileTips
{
    [SerializeField] EnemyGrid grid = default;

    public void ReadEnemyAtt(EnemyAttribute att)
    {
        grid.SetEnemyInfo(att);
    }
}
