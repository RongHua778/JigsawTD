using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : ISceneState
{
	public MenuState(SceneStateController Controller) : base(Controller)
	{
		this.StateName = "MenuState";
	}
	// é_Ê¼
	public override void StateBegin()
	{
		MenuUIManager.Instance.Initinal();
	}

	// ½YÊø
	public override void StateEnd()
	{
		MenuUIManager.Instance.Release();
	}

	public override void StateUpdate()
	{
		MenuUIManager.Instance.GameUpdate();
	}

}
