using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill 
{
    protected Enemy enemy;


    //在enemy出生的时候调用
    public virtual void OnBorn()
    {

    }

    //在enemy的gameupdate中调用
    public virtual void OnGameUpdating()
    {

    }

    //enemy死亡时调用
    public virtual void OnDying()
    {

    }
}
