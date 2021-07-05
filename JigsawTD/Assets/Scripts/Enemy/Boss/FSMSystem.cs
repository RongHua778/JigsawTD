using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private Dictionary<StateID, FSMState> States = new Dictionary<StateID, FSMState>();
    private StateID CurrentStateID;
    private FSMState CurrentState;

    public void Update(Aircraft agent)
    {
        CurrentState.Act(agent);
        CurrentState.Reason(agent);
    }

    public void AddState(FSMState s)
    {
        if (s == null)
        {
            Debug.LogError("state为空");
            return;
        }

        if (CurrentState == null)
        {
            CurrentState = s;
            CurrentStateID = s.StateID;
        }

        if (States.ContainsKey(s.StateID))
        {
            Debug.LogError("状态" + s.StateID + "已经存在，无法重复添加");
            return;
        }

        States.Add(s.StateID, s);
    }

    public void DeleteFSMState(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("无法删除空状态");
            return;
        }

        if (States.ContainsKey(id) == false)
        {
            Debug.LogError("无法删除不存在状态" + id);
            return;
        }

        States.Remove(id);
    }

    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("无法执行空的转换条件");
            return;
        }
        StateID id = CurrentState.GetStateID(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogWarning("当前状态" + CurrentStateID + "无法根据转换条件" + trans + "发生转换");
            return;
        }

        if (States.ContainsKey(id) == false)
        {
            Debug.LogError("状态机内没有包含" + id + "，无法进行状态转换");
            return;
        }

        FSMState state = States[id];
        CurrentState.DoAfterLeaving();
        CurrentState = state;
        CurrentStateID = id;
        CurrentState.DoBeforeEntering();
    }
}