using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlacementState
{
    Default, Picking, None
}

public class ShapeSystem : IGameSystem//��Ҫ����Ϊһ��ϵͳ��������״���׶�
{
    static PlacementState placeState = PlacementState.Default;
    public static PlacementState PlaceState { get => placeState; set => placeState = value; }

    [SerializeField]
    TileSelect[] tileSelects = default;
    [SerializeField]
    GameObject[] areas;//0=��·ģʽ//1=ѡ·ģʽ

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    public void ShowArea(int exceptId)
    {
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(false);
        }
        areas[exceptId].SetActive(true);
        PlaceState = (PlacementState)exceptId;
    }

    public void HideArea()
    {
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(false);
        }
        PlaceState = PlacementState.None;
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
