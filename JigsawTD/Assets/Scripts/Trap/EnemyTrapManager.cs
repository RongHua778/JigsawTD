using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrapManager
{
    public TrapContent trap;
    //public bool trapPassed;
    public EnemyTrapManager(TrapContent trap, bool trapPassed)
    {
        this.trap = trap;
        //this.trapPassed = trapPassed;
    }
}
