using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirProtector : Aircraft
{
    public override void Initiate(AircraftCarrier boss)
    {
        base.Initiate(boss);
        if (fsm == null)
        {
            this.boss = boss;
            //以下是状态机的初始化
            fsm = new FSMSystem();


            FSMState patrolState = new AirProtectorPatrolState(fsm);
            patrolState.AddTransition(Transition.ProtectBoss, StateID.Protect);
            PickRandomDes();

            FSMState protectState = new ProtectState(fsm);
            protectState.AddTransition(Transition.LureTarget, StateID.Lure);

            FSMState lureState = new AirProtectorLureState(fsm);
            lureState.AddTransition(Transition.ProtectBoss, StateID.Protect);

            fsm.AddState(patrolState);
            fsm.AddState(protectState);
            fsm.AddState(lureState);

            GameManager.Instance.nonEnemies.Add(this);
        }
    }

    public override bool GameUpdate()
    {
        fsm.Update(this);
        return base.GameUpdate();
    }


    public void Protect()
    {
        if (isLeader||!isFollowing)
        {
            movingSpeed = originalMovingSpeed;
            rotatingSpeed = originalRotatingSpeed;
            float distanceToTarget = ((Vector2)transform.position - (Vector2)boss.transform.position).magnitude;
            if (distanceToTarget < minDistanceToLure)
            {
                movingDirection = boss.transform.position - transform.position + new Vector3(0.5f, 0.5f);
            }
            else
            {
                movingDirection = boss.transform.position - transform.position;
            }
            MovingToTarget(Destination.Random);
        }
        else
        {
            Follow();
        }

    }


    public override void OnUnSpawn()
    {
        isLeader = false;
        predecessor = null;
        base.OnUnSpawn();
    }
}
