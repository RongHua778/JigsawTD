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

    private int drawRemain = 1;
    public int DrawRemain
    {
        get => drawRemain;
        set
        {
            drawRemain = value;
            DrawBtnTxt.text = "抽取模块X" + drawRemain.ToString();
        }
    }

    private int playerLevel = 1;
    public int PlayerLevel 
    {
        get => playerLevel; 
        set => playerLevel = value; 
    }

    private int luckPoint;
    public int LuckPoint
    {
        get => luckPoint;
        set
        {
            luckPoint = value;
            LuckPointTxt.text = "累积点:" + luckPoint.ToString();
        }
    }

    private int luckProgress;
    public int LuckProgress { get => luckProgress; set => luckProgress = value; }

    //UI
    [SerializeField] Text DrawBtnTxt = default;
    [SerializeField] Text LevelUpBtnTxt = default;
    [SerializeField] Text LuckPointTxt = default;

    [SerializeField]
    TileSelect[] tileSelects = default;
    [SerializeField]
    GameObject[] areas;//0=放路模式//1=选路模式

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    //**********按钮回调
    public void DrawBtnClick()
    {
        if (DrawRemain > 0)
        {
            DrawRemain--;
            drawThisTurn = true;
            ShowArea(1);
            for(int i = 0; i < tileSelects.Length; i++)
            {
                TileShape shape = GameManager.Instance.ShapeFactory.GetRandomShape();
                GameTile specialTile = GameManager.Instance.TileFactory.BuildNormalTile(GameTileContentType.Empty);
                shape.InitializeShape(specialTile);
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
