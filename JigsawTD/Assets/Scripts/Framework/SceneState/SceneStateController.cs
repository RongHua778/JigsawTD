using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateController
{
    private ISceneState m_State;
    private bool m_RunBegin = false;
    public SceneStateController() { }
    public void SetState(ISceneState state)
    {
        if (m_RunBegin)
            return;
        DebugLog.Logger("EnterSceneState" + state.StateName);
        m_State = state;
        m_State.StateBegin();
        m_RunBegin = true;
    }

    public void EndState()
    {
        m_RunBegin = false;
        if (m_State != null)
        {
            m_State.StateEnd();
            m_State = null;
        }
    }


    public void StateUpdate()
    {
        if (m_State != null)
            m_State.StateUpdate();
    }

}
