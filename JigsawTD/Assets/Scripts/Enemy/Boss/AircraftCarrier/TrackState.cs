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
        agent.MovingToTarget(Destination.target);
    }
    public override void Reason(Aircraft agent)
    {
        float distanceToTarget = ((Vector2)agent.transform.position - (Vector2)agent.targetTurret.transform.position).magnitude;
        if(distanceToTarget< agent.minDistanceToDealDamage)
        {
            Debug.LogWarning("Ready to attack targetsTurret:" + agent.targetTurret);
            FrostEffect frosteffect = ObjectPool.Instance.Spawn(agent.frostPrefab) as FrostEffect;
            frosteffect.transform.position = agent.targetTurret.transform.position;
            frosteffect.UnspawnAfterTime(agent.freezeTime);
            agent.targetTurret.Frost(agent.freezeTime);
        }
    }


}
