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
    public List<string> showList=new List<string>();
    public List<string> hideList = new List<string>();
    public override void Trigger()
    {
        foreach (var item in showList)
        {
            GuideGirlUI.GetGuideObj(item).SetActive(true);
        }
        foreach (var item in hideList)
        {
            GuideGirlUI.GetGuideObj(item).SetActive(false);
        }
    }
}

public class ShowIndicator : GuideEvent
{
    public bool show;
    public Transform tr;
    public override void Trigger()
    {
        GuideGirlUI.GuideIndicator.gameObject.SetActive(show);
        if (show)
            GuideGirlUI.GuideIndicator.transform.position = tr.position;
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
    public string RectName;
    public override void Trigger()
    {
        GameManager.Instance.SetRectMaskObj(GuideGirlUI.GetGuideObj(RectName).GetComponent<RectTransform>());
    }
}

public class SetEventPermeaterObj : GuideEvent
{
    public string ObjName;
    public override void Trigger()
    {
        GameManager.Instance.SetEventPermeaterTarget(GuideGirlUI.GetGuideObj(ObjName));
    }
}

public class SetGuideIndicatorPos : GuideEvent
{
    public string ObjName;
    public override void Trigger()
    {
        GameObject guideIndicator = GuideGirlUI.GetGuideObj("GuideIndicator");
        guideIndicator.transform.position = GuideGirlUI.GetGuideObj(ObjName).transform.position;
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

