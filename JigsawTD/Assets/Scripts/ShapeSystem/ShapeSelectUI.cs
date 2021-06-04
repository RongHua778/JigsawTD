using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuiidingState
{
    Default, Picking, None
}

public class ShapeSelectUI : IUserInterface//控制形状生成
{

    [SerializeField] TileSelect[] tileSelects = default;

    public void ShowThreeShapes(int level)
    {
        for (int i = 0; i < tileSelects.Length; i++)
        {
            TileShape shape = ConstructHelper.GetRandomShapeByLevel(level);
            tileSelects[i].InitializeDisplay(i, shape);
        }

    }

    public void ClearAllSelections()
    {
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
