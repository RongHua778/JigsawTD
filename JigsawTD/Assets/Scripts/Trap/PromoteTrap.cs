using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteTrap : TrapContent
{
    //���������������Ч������50%
    public override void OnContentPreCheck(int index, List<BasicTile> path)
    {
        base.OnContentPreCheck(index, path);
        for (int i = 0; i < 3; i++)
        {
            if (path[index + i] != null)
            {
                path[index + i].TrapIntensify += 0.5f;
            }
        }
    }

}