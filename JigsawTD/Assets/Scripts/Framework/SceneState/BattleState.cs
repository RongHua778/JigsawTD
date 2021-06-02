using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : ISceneState
{
	public BattleState(SceneStateController Controller) : base(Controller)
	{
		this.StateName = "BattleState";
	}

	// �_ʼ
	public override void StateBegin()
	{
		GameManager.Instance.Initinal();
	}

	// �Y��
	public override void StateEnd()
	{
		GameManager.Instance.Release();
	}

	// ����
	public override void StateUpdate()
	{
		// �[��߉݋
		GameManager.Instance.GameUpdate();

		// �[���Ƿ�Y��
		//if (GameManager.Instance.ThisGameIsOver())
		//	m_Controller.SetState(new MainMenuState(m_Controller), "MainMenuScene");
	}

}
