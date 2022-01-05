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
		MenuManager.Instance.Initinal();
		Sound.Instance.PlayBg("menu");
	}

	// ½YÊø
	public override void StateEnd()
	{
		MenuManager.Instance.Release();
	}

	public override void StateUpdate()
	{
		MenuManager.Instance.GameUpdate();
	}

}
