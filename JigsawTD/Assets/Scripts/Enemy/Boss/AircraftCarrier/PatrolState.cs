using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    float waitingTime;
    public PatrolState(FSMSystem fsm):base(fsm)
    {
        StateID = StateID.Patrol;
    }
    public override void Act(Aircraft agent)
    {
        agent.MovingToTarget(Destination.Random);
        waitingTime += Time.deltaTime;
        if (waitingTime >2f)
        {
            agent.PickRandomDes();
            waitingTime = 0;
        }
    }
    public override void Reason(Aircraft agent)
    {
        waitingTime += Time.deltaTime;
        if (waitingTime > 10f)
        {
            agent.SearchTarget();
            waitingTime = 0;
        }
    }
}
