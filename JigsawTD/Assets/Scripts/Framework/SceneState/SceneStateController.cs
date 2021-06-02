using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateController
{
    private ISceneState m_State;
    private bool m_RunBegin = false;
    public SceneStateController() { }
    public void SetState(ISceneState state, string LoadSceneName)
    {
        DebugLog.Logger("EnterSceneState" + state.StateName);
        m_RunBegin = false;

        // �d�����
        Game.Instance.LoadScene(LoadSceneName);

        // ֪ͨǰһ��State�Y��
        if (m_State != null)
            m_State.StateEnd();

        // �O��
        m_State = state;
    }

    private void LoadScene(string LoadSceneName)
    {
        if (LoadSceneName == null || LoadSceneName.Length == 0)
            return;
        SceneManager.LoadScene(LoadSceneName);
    }

    public void StateUpdate()
    {
        // �Ƿ�߀���d��

        //if (!SceneManager.LoadSceneAsync(m_State.StateName, LoadSceneMode.Single).isDone)
        //    return;

        // ֪ͨ�µ�State�_ʼ
        if (m_State != null && m_RunBegin == false)
        {
            m_State.StateBegin();
            m_RunBegin = true;
        }

        if (m_State != null)
            m_State.StateUpdate();
    }

    //private IEnumerator StartLoading(string sceneName)
    //{
    //    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    //    op.allowSceneActivation = false;//����������ֶ���ֵ
    //    while (!op.isDone)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        if (op.progress >= 0.9f)//���ؽ��ȴ��ڵ���0.9ʱ˵���������
    //        {
    //            op.allowSceneActivation = true;//�ֶ���ֵΪtrue����ֵΪtrueʱ��isDone�Զ�����ű�
    //        }
    //        if (op.isDone)
    //        {
    //            break;
    //        }
    //    }
    //    yield return null;
    //}
}