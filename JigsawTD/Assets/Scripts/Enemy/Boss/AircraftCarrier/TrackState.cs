using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackState : FSMState
{
    float trackCounter;
    float maxTrackTime=4f;
    public TrackState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Track;
    }
    public override void Act(AirAttacker agent)
    {
        agent.MovingToTarget(Destination.target);
    }
    public override void Reason(AirAttacker agent)
    {
        float distanceToTarget = ((Vector2)agent.transform.position - (Vector2)agent.targetTurret.transform.position).magnitude;
        if(distanceToTarget< agent.minDistanceToDealDamage)
        {
            agent.Attack();
            fsm.PerformTransition(Transition.Attacked);
        }
        trackCounter += Time.deltaTime;
        if (trackCounter > maxTrackTime)
        {
            fsm.PerformTransition(Transition.Attacked);
            trackCounter = 0;
        }
    }


}
