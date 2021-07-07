using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirProtectorPatrolState : FSMState
{
    float protectWaitingTime;
    float directionWaitingTime;
    float protectBossCD = 2.5f + Random.Range(-0.5f, 0.5f);
    float directionCD = 1.5f + Random.Range(-0.5f, 0.5f);
    public AirProtectorPatrolState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Patrol;
    }
    public override void Act(AirProtector agent)
    {
        agent.MovingToTarget(Destination.Random);
        directionWaitingTime += Time.deltaTime;
        if (directionWaitingTime > directionCD)
        {
            agent.PickRandomDes();
            directionWaitingTime = 0;
        }
    }
    public override void Reason(AirProtector agent)
    {
        protectWaitingTime += Time.deltaTime;
        if (protectWaitingTime > protectBossCD)
        {
            fsm.PerformTransition(Transition.ProtectBoss);
            protectWaitingTime = 0;
        }
    }

}
