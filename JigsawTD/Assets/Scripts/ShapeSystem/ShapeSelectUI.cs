using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShapeSelectUI : IUserInterface//控制形状生成
{
    private Animator m_Anim;
    [SerializeField] TileSelect[] tileSelects = default;

    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
    }
    public void ShowThreeShapes()
    {
        Show();

        for (int i = 0; i < tileSelects.Length; i++)
        {
            TileShape shape = GameRes.PreSetShape[i] != null ?
                ConstructHelper.GetTutorialShape(GameRes.PreSetShape[i]) : ConstructHelper.GetRandomShapeByLevel();
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

    public override void Show()
    {
        base.Show();
        m_Anim.SetTrigger("Show");
    }

}
