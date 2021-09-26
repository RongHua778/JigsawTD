using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShapeSelectUI : IUserInterface//控制形状生成
{
    [SerializeField] TileSelect[] tileSelects = default;
    public void ShowThreeShapes(int level)
    {
        Show();

        for (int i = 0; i < tileSelects.Length; i++)
        {
            TileShape shape = GameRes.PreSetShape != null ?
                ConstructHelper.GetTutorialShape(GameRes.PreSetShape) : ConstructHelper.GetRandomShapeByLevel(level);
            tileSelects[i].InitializeDisplay(i, shape);
        }

    }


    public void ClearAllSelections()
    {
        foreach (TileSelect select in tileSelects)
        {
            select.ClearShape();
        }
        Hide();
    }


}
