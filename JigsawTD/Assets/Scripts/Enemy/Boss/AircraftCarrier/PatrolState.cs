using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    float LureProb = 0.4f;
    float waitingTime;
    float searchTargetCD=2.5f+Random.Range(-0.5f,0.5f);
    public PatrolState(FSMSystem fsm):base(fsm)
    {
        StateID = StateID.Patrol;
    }
    public override void Act(Aircraft agent)
    {
        agent.MovingToTarget(Destination.Random);
        waitingTime += Time.deltaTime;
        if (waitingTime % 2==0)
        {
            agent.PickRandomDes();
        }
    }
    public override void Reason(Aircraft agent)
    {
        waitingTime += Time.deltaTime;
        if (waitingTime > searchTargetCD)
        {
            agent.SearchTarget();
            waitingTime = 0;
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
}
