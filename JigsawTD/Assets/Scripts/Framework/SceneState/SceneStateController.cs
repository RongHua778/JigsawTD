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
        DebugLog.Logger("EnterSceneState" + state.StateName);
        m_RunBegin = false;
        // 通知前一個State結束
        if (m_State != null)
            m_State.StateEnd();

        // 設定
        m_State = state;
        m_State.StateBegin();
    }

    private void LoadScene(string LoadSceneName)
    {
        if (LoadSceneName == null || LoadSceneName.Length == 0)
            return;
        SceneManager.LoadScene(LoadSceneName);
    }

    public void StateUpdate()
    {
        m_State.StateUpdate();
    }

    //private IEnumerator StartLoading(string sceneName)
    //{
    //    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    //    op.allowSceneActivation = false;//这个变量是手动赋值
    //    while (!op.isDone)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        if (op.progress >= 0.9f)//加载进度大于等于0.9时说明加载完毕
    //        {
    //            op.allowSceneActivation = true;//手动赋值为true（此值为true时，isDone自动会跟着变
    //        }
    //        if (op.isDone)
    //        {
    //            break;
    //        }
    //    }
    //    yield return null;
    //}
}
