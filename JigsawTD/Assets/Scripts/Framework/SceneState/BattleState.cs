using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : ISceneState
{
	public BattleState(SceneStateController Controller) : base(Controller)
	{
		this.StateName = "BattleState";
	}

	// 開始
	public override void StateBegin()
	{
		GameManager.Instance.Initinal();
	}

	// 結束
	public override void StateEnd()
	{
		GameManager.Instance.Release();
	}

	// 更新
	public override void StateUpdate()
	{
		// 遊戲邏輯
		GameManager.Instance.GameUpdate();

		// 遊戲是否結束
		//if (GameManager.Instance.ThisGameIsOver())
		//	m_Controller.SetState(new MainMenuState(m_Controller), "MainMenuScene");
	}

}
