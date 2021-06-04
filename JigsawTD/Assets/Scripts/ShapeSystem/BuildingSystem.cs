using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuiidingState
{
    Default, Picking, None
}

public class BuildingSystem : IGameSystem//控制形状，升级
{
    static BuiidingState buildingState = BuiidingState.Default;
    public static BuiidingState BuildingState { get => buildingState; set => buildingState = value; }

    bool drawThisTurn=false;

    [SerializeField] TileSelect[] tileSelects = default;
    [SerializeField] GameObject[] areas;//0=放路模式//1=选路模式

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        GameEvents.Instance.onConfirmShape += ConfirmShape;
    }

    private void ConfirmShape()
    {
        ShowArea(0);
    }

    //**********按钮回调
    public void DrawBtnClick()
    {
        if (ResourcesManager.Instance.DrawRemain > 0)
        {
            ResourcesManager.Instance.DrawRemain--;
            drawThisTurn = true;
            ShowArea(1);
            for(int i = 0; i < tileSelects.Length; i++)
            {

                TileShape shape = ConstructHelper.GetRandomShapeByLevel();
                tileSelects[i].InitializeDisplay(i, shape);
            }
        }
        else
        {
            GameEvents.Instance.Message("抽取次数不足");
        }
    }

    public void ShowArea(int exceptId)
    {
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(false);
        }
        areas[exceptId].SetActive(true);
        BuildingState = (BuiidingState)exceptId;
    }

    public void HideArea()
    {
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(false);
        }
        BuildingState = BuiidingState.None;
    }

    public void ClearAllSelections()
    {
        HideArea();
        foreach (TileSelect select in tileSelects)
        {
            select.ClearShape();
        }
    }

    public void DisplayShapeOnTileSelct(int displayID, TileShape shape)
    {
        tileSelects[displayID].InitializeDisplay(displayID, shape);
    }

}
