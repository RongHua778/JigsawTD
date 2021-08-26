using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeInfo
{
    public ShapeType ShapeType;
    public Element Element;
    public int Quality;
    public int TurretPos;
    public Vector2 ForcePos;
    public Vector2 ForceDir;
    public List<Vector2> TutorialPoss;

    public ShapeInfo(ShapeType type,Element element,int quality,int pos,Vector2 forcePos,Vector2 forceDir,List<Vector2> tPoss)
    {
        this.ShapeType = type;
        this.Element = element;
        this.Quality = quality;
        this.TurretPos = pos;
        this.ForcePos = forcePos;
        this.ForceDir = forceDir;
        this.TutorialPoss = tPoss;
    }
}
public class ShapeSelectUI : IUserInterface//控制形状生成
{
    [SerializeField] TileSelect[] tileSelects = default;

    ShapeInfo preSetShape;
    public ShapeInfo PreSetShape { get => preSetShape; set => preSetShape = value; }

    public void ShowThreeShapes(int level)
    {
        Show();

        for (int i = 0; i < tileSelects.Length; i++)
        {
            TileShape shape = PreSetShape != null ?
                ConstructHelper.GetTutorialShape(PreSetShape) : ConstructHelper.GetRandomShapeByLevel(level);
            tileSelects[i].InitializeDisplay(i, shape);
        }
        PreSetShape = null;


        //switch (tutorialID)
        //{
        //    case 1://教程第一次抽取,Z型蓝色
        //        for (int i = 0; i < tileSelects.Length; i++)
        //        {
        //            TileShape shape = ConstructHelper.GetTutorialShape(ShapeType.Z, Element.Water, 1, 0, new Vector2(1, 1), Vector2.left);
        //            tileSelects[i].InitializeDisplay(i, shape);
        //            shape.m_ShapeSelectUI = this;
        //        }
        //        List<Vector2> poss1 = new List<Vector2> { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 2) };
        //        SetTutorialPrefabs(poss1);
        //        TutorialID = 0;
        //        break;
        //    case 2://教程第二次抽取，L型金色
        //        for (int i = 0; i < tileSelects.Length; i++)
        //        {
        //            TileShape shape = ConstructHelper.GetTutorialShape(ShapeType.L, Element.Gold, 1, 1, new Vector2(0, 2), Vector2.down);
        //            tileSelects[i].InitializeDisplay(i, shape);
        //            shape.m_ShapeSelectUI = this;
        //        }
        //        List<Vector2> poss2 = new List<Vector2> { new Vector2(0, 1), new Vector2(0, 2), new Vector2(-1, 2), new Vector2(-2, 2) };
        //        SetTutorialPrefabs(poss2);
        //        TutorialID = 0;
        //        break;
        //    case 3://教程第三次抽取，T型红色
        //        for (int i = 0; i < tileSelects.Length; i++)
        //        {
        //            TileShape shape = ConstructHelper.GetTutorialShape(ShapeType.T, Element.Fire, 1, 3, new Vector2(-2, 0), Vector2.left);
        //            tileSelects[i].InitializeDisplay(i, shape);
        //            shape.m_ShapeSelectUI = this;
        //        }
        //        List<Vector2> poss3 = new List<Vector2> { new Vector2(-2, 0), new Vector2(-2, 1), new Vector2(-2, -1), new Vector2(-3, 0) };
        //        SetTutorialPrefabs(poss3);
        //        TutorialID = 0;
        //        break;
        //    default:
        //        for (int i = 0; i < tileSelects.Length; i++)
        //        {
        //            TileShape shape = ConstructHelper.GetRandomShapeByLevel(level);
        //            tileSelects[i].InitializeDisplay(i, shape);
        //        }
        //        break;
        //}

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
