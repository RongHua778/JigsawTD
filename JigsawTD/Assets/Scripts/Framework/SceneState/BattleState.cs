using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : ISceneState
{
	public BattleState(SceneStateController Controller) : base(Controller)
	{
		this.StateName = "BattleState";
	}

	// é_Ê¼
	public override void StateBegin()
	{
		GameManager.Instance.Initinal();
	}

	// ½YÊø
	public override void StateEnd()
	{
		GameManager.Instance.Release();
	}

	// ¸üÐÂ
	public override void StateUpdate()
	{
		GameManager.Instance.GameUpdate();
	}

}
