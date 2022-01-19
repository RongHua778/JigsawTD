using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTrap : TrapContent
{

    //public override void OnContentPreCheck(int index, List<BasicTile> path)
    //{
    //    base.OnContentPreCheck(index, path);
    //    for (int i = 0; i < 3; i++)
    //    {
    //        if ((index + i) < path.Count)
    //        {
    //            path[index + i].BounsCoin += Mathf.RoundToInt(5 * m_GameTile.TrapIntensify);
    //        }
    //    }
    //}

    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        base.OnContentPass(enemy, content);
        int amount = Mathf.RoundToInt(3 * TrapIntensify * enemy.DamageStrategy.TrapIntensify);
        CoinAnalysis += amount;
        StaticData.Instance.GainMoneyEffect(enemy.model.position, amount);
        enemy.DamageStrategy.TrapIntensify = 1;
    }
}
