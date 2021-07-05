using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    Enemy boss;
    float waitingTime;
    public PatrolState(FSMSystem fsm, Enemy boss):base(fsm)
    {
        StateID = StateID.Patrol;
        this.boss = boss;
    }
    public override void Act(Aircraft agent)
    {
        agent.transform.RotateAround(boss.transform.localPosition,
            boss.transform.forward, Time.deltaTime * agent.movingSpeed);

        //agent.transform.Translate(Vector3.up * Time.deltaTime);

    }
    public override void Reason(Aircraft agent)
    {
        waitingTime += Time.deltaTime;
        if (waitingTime > 2f)
        {
            int hits = Physics2D.OverlapCircleNonAlloc(agent.transform.position,
                 agent.exploreRange, agent.attachedResult, LayerMask.GetMask(StaticData.TurretMask));
            if (hits > 0)
            {
                int temp = Random.Range(0, hits);
                agent.targetTurret = agent.attachedResult[temp].GetComponent<TurretContent>();
                if (agent.targetTurret != null)
                {
                    fsm.PerformTransition(Transition.WaitingEnough, agent);
                }
            }
            waitingTime = 0;
        }
    }
}
