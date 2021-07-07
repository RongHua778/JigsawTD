using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackerPatrolState : FSMState
{
    float LureProb = 0.4f;
    float attackWaitingTime;
    float directionWaitingTime;
    float searchTargetCD=2.5f+Random.Range(-0.5f,0.5f);
    float directionCD=1.5f+Random.Range(-0.5f,0.5f);
    public AirAttackerPatrolState(FSMSystem fsm):base(fsm)
    {
        StateID = StateID.Patrol;
    }
    public override void Act(AirAttacker agent)
    {
        agent.MovingToTarget(Destination.Random);
        directionWaitingTime += Time.deltaTime;
        if (directionWaitingTime > directionCD)
        {
            agent.PickRandomDes();
            directionWaitingTime = 0;
        }
    }
    public override void Reason(AirAttacker agent)
    {
        attackWaitingTime += Time.deltaTime;
        if (attackWaitingTime > searchTargetCD)
        {
            agent.SearchTarget();
            attackWaitingTime = 0;
        }
        if (agent.targetTurret != null)
        {
            float temp = Random.Range(0f, 1f);
            if (temp < LureProb)
            {
                fsm.PerformTransition(Transition.LureTarget);
            }
            else
            {
                fsm.PerformTransition(Transition.AttackTarget);
            }
        }
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
        searchTargetCD = 2.5f + Random.Range(-1f, 1f);
        directionCD = 1.5f + Random.Range(-1f, 1f);
    }
}
