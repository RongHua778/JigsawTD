using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : ISceneState
{
	public MenuState(SceneStateController Controller) : base(Controller)
	{
		this.StateName = "MenuState";
	}
	// �_ʼ
	public override void StateBegin()
	{
		MenuUIManager.Instance.Initinal();
	}

	// �Y��
	public override void StateEnd()
	{
		MenuUIManager.Instance.Release();
	}

	public override void StateUpdate()
	{
		MenuUIManager.Instance.GameUpdate();
	}

}
