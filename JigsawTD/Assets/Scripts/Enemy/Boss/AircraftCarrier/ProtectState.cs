using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectState : FSMState
{
    float waitingTime;
    float protectTime = 5f;
    public ProtectState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Protect;
    }
    public override void Act(Aircraft agent)
    {
        agent.Protect();

    }
    public override void Reason(Aircraft agent)
    {
        waitingTime += Time.deltaTime;
        if (waitingTime > protectTime)
        {
            fsm.PerformTransition(Transition.Patrol);
            waitingTime = 0;
        }
    }

}
