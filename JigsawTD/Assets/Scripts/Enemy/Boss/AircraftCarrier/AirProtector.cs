using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirProtector : Aircraft
{
    public override void Initiate(AircraftCarrier boss, float maxHealth)
    {
        base.Initiate(boss, maxHealth);
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

}
