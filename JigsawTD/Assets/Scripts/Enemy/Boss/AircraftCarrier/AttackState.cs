using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState
{
    float freezeRange = 10f;
    public AttackState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Attack;
    }
    public override void Act(Aircraft agent)
    {

    }
    public override void Reason(Aircraft agent)
    {

    }

    public override void DoBeforeEntering()
    {


        //for (int i = 0; i < hits; i++)
        //{
        //    TurretContent turret = attachedResult[i].GetComponent<TurretContent>();
        //    FrostEffect frosteffect = ObjectPool.Instance.Spawn(frostPrefab) as FrostEffect;
        //    frosteffect.transform.position = attachedResult[i].transform.position;
        //    frosteffect.UnspawnAfterTime(freezeTime);
        //    turret.Frost(freezeTime);
        //}
    }
}
