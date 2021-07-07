using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttacker : Aircraft
{
    [SerializeField] ParticalControl attackPrefab = default;
    [SerializeField] FrostEffect frostPrefab = default;
    float freezeTime = 5f;

    public override void Initiate(AircraftCarrier boss)
    {
        base.Initiate(boss);
        if (fsm == null)
        {
            this.boss = boss;
            //以下是状态机的初始化
            fsm = new FSMSystem();

            FSMState patrolState = new PatrolState(fsm);
            patrolState.AddTransition(Transition.AttackTarget, StateID.Track);
            patrolState.AddTransition(Transition.LureTarget, StateID.Lure);
            PickRandomDes();

            FSMState trackState = new TrackState(fsm);
            trackState.AddTransition(Transition.Attacked, StateID.Back);

            FSMState lureState = new LureState(fsm);
            lureState.AddTransition(Transition.Attacked, StateID.Back);

            FSMState backState = new BackState(fsm);
            backState.AddTransition(Transition.BackToBoss, StateID.Patrol);

            fsm.AddState(patrolState);
            fsm.AddState(trackState);
            fsm.AddState(backState);
            fsm.AddState(lureState);
            GameManager.Instance.nonEnemies.Add(this);
        }
    }
    public override bool GameUpdate()
    {
        fsm.Update(this);
        return base.GameUpdate();
    }
    public void SearchTarget()
    {
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position,
     exploreRange, attachedResult, LayerMask.GetMask(StaticData.TurretMask));
        if (hits > 0)
        {
            List<TurretContent> turrets = new List<TurretContent>();
            for (int i = 0; i < hits; i++)
            {
                if (attachedResult[i].GetComponent<TurretContent>().Activated)
                {
                    turrets.Add(attachedResult[i].GetComponent<TurretContent>());
                }
            }
            if (turrets.Count > 0)
            {
                int temp = Random.Range(0, turrets.Count);
                targetTurret = turrets[temp];
            }
        }
    }

    public void Attack()
    {
        FrostEffect frosteffect = ObjectPool.Instance.Spawn(frostPrefab) as FrostEffect;
        frosteffect.transform.position = targetTurret.transform.position;
        frosteffect.UnspawnAfterTime(freezeTime);
        targetTurret.Frost(freezeTime);
        ReusableObject explosion = ObjectPool.Instance.Spawn(attackPrefab);
        Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
        explosion.transform.position = targetTurret.transform.position;
        targetTurret = null;
    }

    public void Lure()
    {
        float distanceToTarget = ((Vector2)transform.position - (Vector2)targetTurret.transform.position).magnitude;
        if (distanceToTarget < minDistanceToLure)
        {
            movingDirection = targetTurret.transform.position - transform.position + new Vector3(0.5f, 0.5f);
            MovingToTarget(Destination.Random);
        }
        else
        {
            movingDirection = targetTurret.transform.position - transform.position;
            MovingToTarget(Destination.Random);
        }
    }
}
