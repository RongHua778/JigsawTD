using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlacementState
{
    Default, Picking, None
}

public class RoadPlacement : MonoBehaviour//需要独立为一个系统，控制形状，阶段
{
    private static PlacementState placeState = PlacementState.Default;

    [SerializeField]
    TileSelect[] tileSelects = default;
    [SerializeField]
    GameObject[] areas;//0=放路模式//1=选路模式

    public static PlacementState PlaceState { get => placeState; set => placeState = value; }

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
