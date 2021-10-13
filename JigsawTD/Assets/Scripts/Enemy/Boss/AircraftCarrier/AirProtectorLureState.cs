using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirProtectorLureState : FSMState
{
    float waitingTime;
    float lureTime = 4f + Random.Range(0, 2f);
    public AirProtectorLureState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Lure;
    }
    public override void Act(Aircraft agent)
    {
        agent.Lure();
    }
    public override void Reason(Aircraft agent)
    {
        waitingTime += Time.deltaTime;
        if (waitingTime > lureTime)
        {
            fsm.PerformTransition(Transition.ProtectBoss);
            agent.targetTurret = null;
            waitingTime = 0;
        }
    }
}
