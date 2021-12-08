using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GuideEvent
{
    public virtual void Trigger()
    {

    }
}

public class ShowGameobject : GuideEvent
{
    public List<string> showList = new List<string>();
    public List<string> hideList = new List<string>();
    public override void Trigger()
    {
        foreach (var item in showList)
        {
            GameManager.Instance.GetGuideObj(item).SetActive(true);
        }
        foreach (var item in hideList)
        {
            GameManager.Instance.GetGuideObj(item).SetActive(false);
        }
    }
}

public class ManualSetSquence : GuideEvent
{
    public EnemyType EnemyType;
    public float Stage;
    public int Wave;
    public override void Trigger()
    {
        GameManager.Instance.ManualSetSequence(EnemyType, Stage, Wave);
    }
}

public class SetMouseTutorial : GuideEvent
{
    public bool MoveAble;
    public bool MoveTurorial;
    public bool SizeTutorial;
    public override void Trigger()
    {
        GameManager.Instance.SetCamMovable(MoveAble);
        GameManager.Instance.SetMoveTutorial(MoveTurorial);
        GameManager.Instance.SetSizeTutorial(SizeTutorial);
    }
}

public class PlayMainUIAnim : GuideEvent
{
    public int partID;
    public string key;
    public bool value;

    public override void Trigger()
    {
        MainUI.PlayMainUIAnim(partID, key, value);
    }
}

public class RectMaskObj : GuideEvent//’⁄’÷Ãÿ∂®«¯”Ú
{
    public bool Show;
    public string RectName;
    public float delayTime;
    public override void Trigger()
    {
        GameManager.Instance.SetRectMaskObj(Show ? GameManager.Instance.GetGuideObj(RectName) : null, delayTime);
    }
}

public class SetEventPermeaterObj : GuideEvent
{
    public bool Show;
    public string ObjName;
    public override void Trigger()
    {
        GameManager.Instance.SetEventPermeaterTarget(Show ? GameManager.Instance.GetGuideObj(ObjName) : null);
    }
}

public class SetGuideIndicatorPos : GuideEvent
{
    public bool Show;
    public string ObjName;
    public override void Trigger()
    {
        GuideIndicator guideIndicator = GameManager.Instance.GetGuideObj("GuideIndicator").GetComponent<GuideIndicator>();
        guideIndicator.Show(Show, Show ? GameManager.Instance.GetGuideObj(ObjName) : null);
    }
}
public class PlayFuncUIAnim : GuideEvent
{
    public int partID;
    public string key;
    public bool value;

    public override void Trigger()
    {
        FuncUI.PlayFuncUIAnim(partID, key, value);
    }
}

public class SetPresetShape : GuideEvent
{
    public ShapeInfo PresetShape;
    public int ShapeSlot;

    public override void Trigger()
    {
        GameRes.PreSetShape[ShapeSlot] = PresetShape;
    }
}

public class SetForcePlace : GuideEvent
{
    public ForcePlace ForcePlace;

    public override void Trigger()
    {
        GameRes.ForcePlace = ForcePlace;
    }
}

public class ShowGuideBook : GuideEvent
{
    public int PageID;
    public override void Trigger()
    {
        GameManager.Instance.ShowGuideVideo(PageID);
    }
}


public class EndTutorial : GuideEvent
{
    public override void Trigger()
    {
        Game.Instance.Tutorial = false;
    }
}

public class ShowGuideGirl : GuideEvent
{
    public bool Show;
    public int PosID;
    public override void Trigger()
    {
        GameManager.Instance.ShowGuideGirl(Show, PosID);
    }
}

public class GainGold : GuideEvent
{
    public int Amount;
    public override void Trigger()
    {
        GameRes.Coin += Amount;
    }
}

public class AddBluePrint : GuideEvent
{
    public string TurretName;
    public List<int> Elements;
    public List<int> Qualities;

    public override void Trigger()
    {
        RefactorStrategy strategy = ConstructHelper.GetSpecificStrategy(TurretName, Elements, Qualities);
        GameManager.Instance.AddBluePrint(strategy);
    }
}

public class RemoveBluePrint : GuideEvent
{
    public int ID;
    public override void Trigger()
    {
        GameManager.Instance.RemoveBluePrint(ID);
    }
}

