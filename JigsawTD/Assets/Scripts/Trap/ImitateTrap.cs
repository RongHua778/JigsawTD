using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImitateTrap : TrapContent
{

    TrapContent lastTrap;

    public override void OnContentPreCheck(int index, List<BasicTile> path)
    {
        if (BoardSystem.LastTrap != null)
        {
            lastTrap = BoardSystem.LastTrap;
            lastTrap.OnContentPreCheck(index, path);
        }
    }

    public override void OnContentPass(Enemy enemy)
    {
        if (lastTrap != null)
            lastTrap.OnContentPass(enemy);
    }
}
