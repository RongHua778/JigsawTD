using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImitatingTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy)
    {
        //int lastIndex = enemy.PassedTraps.Count - 1;
        //TrapContent previousTrap;
        //if (lastIndex > 0)
        //{
        //    previousTrap = enemy.PassedTraps[lastIndex];
        //    passingOnce = previousTrap.passingOnce;
        //}
        base.OnContentPass(enemy);
    }
    public override void PassManyTimes(Enemy enemy)
    {
        base.PassManyTimes(enemy);
        int lastIndex = enemy.PassedTraps.Count-2;
        TrapContent previousTrap;
        if (lastIndex >= 0) 
        {
            previousTrap = enemy.PassedTraps[lastIndex];
            //while (previousTrap == this)
            //{
            //    lastIndex -= 1;
            //    if (lastIndex < 0) return;
            //    previousTrap = enemy.PassedTraps[lastIndex];
            //}
            if (previousTrap == this)
            {
               return;
            }
            if (previousTrap.passingOnce)
            {
                previousTrap.OnPassOnce(enemy);
                previousTrap.OnLeaveOnce(enemy);
            }
            else
            {
                previousTrap.PassManyTimes(enemy);
                previousTrap.OnLeaveManyTimes(enemy);
            }

        }
    }

    public override void OnPassOnce(Enemy enemy)
    {
        base.OnPassOnce(enemy);
        ////enemy.PassedTraps.Add(this);
        //int lastIndex = enemy.PassedTraps.Count - 2;
        //TrapContent previousTrap;
        //if (lastIndex >= 0)
        //{
        //    previousTrap = enemy.PassedTraps[lastIndex];
        //    previousTrap.OnPassOnce(enemy);
        //    previousTrap.OnLeaveOnce(enemy);
        //}
    }

    public override void OnLeaveOnce(Enemy enemy)
    {

    }

    public override void OnLeaveManyTimes(Enemy enemy)
    {

    }
}
