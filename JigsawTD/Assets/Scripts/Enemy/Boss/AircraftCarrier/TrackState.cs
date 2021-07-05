using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackState : FSMState
{
    public TrackState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Track;
    }
    public override void Act(Aircraft agent)
    {
        RotateTowards(agent);
        agent.transform.Translate(Vector3.up * Time.deltaTime*agent.movingSpeed);
    }
    public override void Reason(Aircraft agent)
    {
        float distanceToTarget = ((Vector2)agent.transform.position - (Vector2)agent.targetTurret.transform.position).magnitude;
        if(distanceToTarget< agent.minDistanceToDealDamage)
        {
            fsm.PerformTransition(Transition.ReadyForAttack,agent);
        }
    }

    private void RotateTowards(Aircraft agent)
    {
        var dir = agent.targetTurret.transform.position - agent.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        agent.look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, agent.look_Rotation, Time.deltaTime);
    }
}
