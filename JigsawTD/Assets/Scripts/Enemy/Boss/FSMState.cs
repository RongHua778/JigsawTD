using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition//转换条件
{
    NullTransition = 0,
    AttackTarget,
    LureTarget,
    Attacked,
    BackToBoss,
    ProtectBoss,
    Patrol
}

public enum StateID//状态id
{
    NullStateID = 0,
    Patrol,
    Track,
    Back,
    Lure,
    Protect
}

//作为基类被其他类继承的有限机状态
public abstract class FSMState
{
    private StateID stateID;
    protected FSMSystem fsm;
    public StateID StateID { get => stateID; set => stateID = value; }

    public FSMState(FSMSystem fsm)
    {
        this.fsm = fsm;
    }


    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();


    public void AddTransition(Transition trans, StateID id)//添加状态
    {
        //安全校验
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("不允许NullTransition");
            return;
        }
        if (id == StateID.NullStateID)
        {
            Debug.LogError("不允许NullStateID");
            return;
        }
        if (map.ContainsKey(trans))
        {
            Debug.LogError("添加转换条件时，" + trans + "已经存在于map中");
            return;
        }
        //校验通过后添加map
        map.Add(trans, id);

    }

    public void DeleteTransition(Transition trans)//删除状态
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("不允许NullTransition");
            return;
        }
        if (map.ContainsKey(trans) == false)
        {
            Debug.LogError("添加转换条件时，" + trans + "不存在于map中");
            return;
        }

        map.Remove(trans);
    }

    public StateID GetStateID(Transition trans)//得到当前状态
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }

        return StateID.NullStateID;
    }


    public virtual void DoBeforeEntering() { }
    public virtual void DoAfterLeaving() { }
    public virtual void Act(Aircraft agent) { }
    public virtual void Reason(Aircraft agent) { }//判断转换条件
    //public virtual void Act(AirProtector agent) { }
    //public virtual void Reason(AirProtector agent) { }//判断转换条件

}