using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectState : FSMState
{
    float waitingTime;
    float protectTime = 15f + Random.Range(0, 2f);
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
            //agent.isFollowing = false;
            fsm.PerformTransition(Transition.Patrol);
            //agent.SearchTarget();
            //if (agent.targetTurret != null)
            //{
                
            //    Debug.LogWarning("found target!");
            //}
            waitingTime = 0;
        }
    }

}
