using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectState : FSMState
{
    float waitingTime;
    float protectTime = 20f + Random.Range(0, 2f);
    public ProtectState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Protect;
    }
    public override void Act(AirProtector agent)
    {
        agent.Protect();

    }
    public override void Reason(AirProtector agent)
    {
        waitingTime += Time.deltaTime;
        if (waitingTime > protectTime)
        {
            agent.SearchTarget();
            if (agent.targetTurret != null)
            {
                fsm.PerformTransition(Transition.LureTarget);
                Debug.LogWarning("found target!");
            }
            waitingTime = 0;
        }
    }

}
