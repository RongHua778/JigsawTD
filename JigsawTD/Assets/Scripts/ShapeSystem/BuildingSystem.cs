using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuiidingState
{
    Default, Picking, None
}

public class BuildingSystem : IGameSystem//������״������
{
    static BuiidingState buildingState = BuiidingState.Default;
    public static BuiidingState BuildingState { get => buildingState; set => buildingState = value; }

    bool drawThisTurn=false;

    [SerializeField] TileSelect[] tileSelects = default;
    [SerializeField] GameObject[] areas;//0=��·ģʽ//1=ѡ·ģʽ

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        GameEvents.Instance.onConfirmShape += ConfirmShape;
    }

    private void ConfirmShape()
    {
        ShowArea(0);
    }

    //**********��ť�ص�
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
            GameEvents.Instance.Message("��ȡ��������");
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
