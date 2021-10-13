using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttacker : Aircraft
{
    [SerializeField] ParticalControl attackPrefab = default;
    float freezeTime = 5f;

    public override void Initiate(AircraftCarrier boss)
    {
        base.Initiate(boss);
        if (fsm == null)
        {
            this.boss = boss;
            //以下是状态机的初始化
            fsm = new FSMSystem();

            FSMState patrolState = new AirAttackerPatrolState(fsm);
            patrolState.AddTransition(Transition.AttackTarget, StateID.Track);
            patrolState.AddTransition(Transition.LureTarget, StateID.Lure);
            patrolState.AddTransition(Transition.ProtectBoss, StateID.Protect);
            PickRandomDes();

            FSMState trackState = new TrackState(fsm);
            trackState.AddTransition(Transition.Attacked, StateID.Back);
            trackState.AddTransition(Transition.ProtectBoss, StateID.Protect);

            FSMState lureState = new AirAttackerLureState(fsm);
            lureState.AddTransition(Transition.Attacked, StateID.Back);
            lureState.AddTransition(Transition.ProtectBoss, StateID.Protect);

            FSMState backState = new BackState(fsm);
            backState.AddTransition(Transition.ProtectBoss, StateID.Protect);
            backState.AddTransition(Transition.Patrol, StateID.Patrol);


            FSMState protectState = new ProtectState(fsm);
            protectState.AddTransition(Transition.Patrol, StateID.Patrol);
            protectState.AddTransition(Transition.ProtectBoss, StateID.Protect);


            fsm.AddState(patrolState);
            fsm.AddState(trackState);
            fsm.AddState(backState);
            fsm.AddState(lureState);
            fsm.AddState(protectState);
            GameManager.Instance.nonEnemies.Add(this);
        }
    }
    public override bool GameUpdate()
    {
        fsm.Update(this);
        return base.GameUpdate();
    }

    public override void Attack()
    {
        //FrostEffect frosteffect = ObjectPool.Instance.Spawn(frostPrefab) as FrostEffect;
        //frosteffect.transform.position = targetTurret.transform.position;
        //frosteffect.UnspawnAfterTime(freezeTime);
        //targetTurret.Frost(freezeTime);
        //ReusableObject explosion = ObjectPool.Instance.Spawn(attackPrefab);
        //Sound.Instance.PlayEffect(explosionClip);
        //explosion.transform.position = targetTurret.transform.position;
        //targetTurret = null;

        FrostEffect frosteffect = null;
        if (targetTurret.Activated)
        {
            frosteffect = ObjectPool.Instance.Spawn(StaticData.Instance.FrostEffectPrefab) as FrostEffect;
            frosteffect.transform.position = targetTurret.transform.position;
        }
        targetTurret.Frost(freezeTime, frosteffect);
        targetTurret = null;

    }



}
