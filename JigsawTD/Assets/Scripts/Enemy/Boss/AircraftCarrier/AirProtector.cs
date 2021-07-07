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
            //������״̬���ĳ�ʼ��
            fsm = new FSMSystem();

            FSMState protectState = new ProtectState(fsm);
            protectState.AddTransition(Transition.ProtectBoss, StateID.Protect);

            fsm.AddState(protectState);

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
        if (isLeader)
        {
            float distanceToTarget = ((Vector2)transform.position - (Vector2)boss.transform.position).magnitude;
            if (distanceToTarget < minDistanceToLure)
            {
                movingDirection = boss.transform.position - transform.position + new Vector3(0.5f, 0.5f);
            }
            else
            {
                movingDirection = boss.transform.position - transform.position;
            }
        }
        else
        {
            movingDirection = predecessor.tail.position - transform.position;
        }
        MovingToTarget(Destination.Random);
    }


    public override void OnUnSpawn()
    {
        isLeader = false;
        predecessor = null;
        base.OnUnSpawn();
    }
}