using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackState : FSMState
{
    public BackState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Back;
    }
    public override void Act(AirAttacker agent)
    {
        agent.MovingToTarget(Destination.boss);
    }
    public override void Reason(AirAttacker agent)
    {
        float distanceToTarget = ((Vector2)agent.transform.position - 
            (Vector2)agent.boss.transform.position).magnitude;
        if (distanceToTarget < 1f)
        {
            fsm.PerformTransition(Transition.BackToBoss);
        }
    }

    public override void DoBeforeEntering()
    {

    }
}
