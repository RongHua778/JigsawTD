using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class BluePrintShopUI : IUserInterface
{
    bool Showing = false;//控制动画
    Animator anim;
    public GameObject ShopBtnObj;

    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;
    [SerializeField] Text NextRefreshTurnsTxt = default;
    [SerializeField] Transform shopContent = default;
    [SerializeField] Text PerfectElementTxt = default;
    [SerializeField] InfoBtn PerfectInfo = default;

    public List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();//商店配方表
    public List<BluePrintGrid> OwnBluePrints = new List<BluePrintGrid>();//拥有配方表

    int shopCapacity = 3;
    public int ShopCapacity { get => shopCapacity; set => shopCapacity = value; }
    int nextRefreshTrun = 0;
    public int NextRefreshTrun //下次自动刷新回合
    {
        get => nextRefreshTrun;
        set
        {
            nextRefreshTrun = value;
            if (nextRefreshTrun <= 0)
            {
                nextRefreshTrun = 3;
                GameManager.Instance.RefreshShop(0);
            }
            NextRefreshTurnsTxt.text = GameMultiLang.GetTraduction("NEXTREFRESH")+":"+nextRefreshTrun + GameMultiLang.GetTraduction("WAVE");
        }
    }



    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        anim = this.GetComponent<Animator>();
        NextRefreshTrun = 4;
        SetPerfectElementCount(0);
        PerfectInfo.SetContent(GameMultiLang.GetTraduction("PERFECTINFO"));
    }

    public void PrepareForGuide()
    {
        ShopBtnObj.SetActive(false);
    }

    public void SetPerfectElementCount(int count)
    {
        PerfectElementTxt.text = GameMultiLang.GetTraduction("OWNPERFECT") + ":" + count;
    }

    public void RefreshShop(int level, int cost)//刷新商店
    {
        foreach (var grid in ShopBluePrints.ToList())
        {
            RemoveGrid(grid);
        }
        ShopBluePrints.Clear();
        for (int i = 0; i < ShopCapacity; i++)
        {
            Blueprint bluePrint = ConstructHelper.GetRandomBluePrintByLevel(level);
            AddBluePrint(bluePrint, true);
        }
    }

    public void AddBluePrint(Blueprint bluePrint, bool isShopBluePrint)//增加蓝图，isShopblueprint判断加入商店还是拥有
    {
        BluePrintGrid bluePrintGrid = ObjectPool.Instance.Spawn(bluePrintGridPrefab) as BluePrintGrid;
        bluePrintGrid.transform.SetParent(shopContent);
        bluePrintGrid.transform.localScale = Vector3.one;
        bluePrintGrid.transform.localPosition = Vector3.zero;
        bluePrintGrid.SetElements(shopContent.GetComponent<ToggleGroup>(), bluePrint);
        if (isShopBluePrint)
        {
            bluePrintGrid.transform.SetAsFirstSibling();
            bluePrintGrid.InShop = true;
            ShopBluePrints.Add(bluePrintGrid);
        }
        else
        {
            MoveBluePrintToPocket(bluePrintGrid);
        }
    }

    public void ShopBtnClick()//播放商店界面打开动画
    {
        Showing = !Showing;
        if (Showing)
            Show();
        else
            Hide();
    }

    public override void Hide()
    {
        anim.SetBool("Showing", false);
    }

    public override void Show()
    {
        anim.SetBool("Showing", true);
    }

    public void MoveBluePrintToPocket(BluePrintGrid grid)//把商店配方移入拥有
    {
        if (StaticData.NextBuyIntensifyBlueprint > 0)//下一个购买的是强化配方
        {
            StaticData.NextBuyIntensifyBlueprint--;
            grid.BluePrint.IntensifyBluePrint = true;
            grid.BluePrint.SetBluePrintIntensify();
            grid.BluePrint.ComStrategy.ClearBasicIntensify();
            grid.BluePrint.ComStrategy.SetQualityValue();
            grid.BluePrint.ComStrategy.GetComIntensify();
            grid.BluePrint.ComStrategy.GetTurretSkills();
        }
        grid.InShop = false;
        grid.transform.SetAsLastSibling();
        OwnBluePrints.Add(grid);
        if (ShopBluePrints.Contains(grid))
            ShopBluePrints.Remove(grid);
        grid.transform.SetAsLastSibling();
    }

    public void RefreshBtnClick()
    {
        GameManager.Instance.RefreshShop(StaticData.Instance.ShopRefreshCost);
    }



    public void CompositeBluePrint(BluePrintGrid grid)//合成对应的配方
    {
        grid.BluePrint.BuildBluePrint();
        ConstructHelper.GetCompositeTurretByBluePrint(grid.BluePrint);
        RemoveGrid(grid);
        CheckAllBluePrint();
        GameManager.Instance.HideTips();
        //设置结算信息
        GameEndUI.TotalComposite++;
    }

    private void RemoveGrid(BluePrintGrid grid)//移除对应的配方，并清理列表
    {
        if (ShopBluePrints.Contains(grid))
        {
            ShopBluePrints.Remove(grid);
        }
        if (OwnBluePrints.Contains(grid))
        {
            OwnBluePrints.Remove(grid);
        }
        ObjectPool.Instance.UnSpawn(grid);
    }

    public void CheckAllBluePrint()//检查所有配方是否达成合成条件
    {
        foreach (var bluePrint in ShopBluePrints)
        {
            bluePrint.CheckElements();
        }
        foreach (var bluePrint in OwnBluePrints)
        {
            bluePrint.CheckElements();
        }
    }

    public void GetARandomBluePrintToPocket(int level, bool isIntensify = false)//幸运值满，随机获得一个配方
    {
        Blueprint bluePrint = ConstructHelper.GetRandomBluePrintByLevel(level, isIntensify);
        AddBluePrint(bluePrint, false);
    }

    public void PreviewComposition(bool value, Element element, int quality)
    {
        foreach (var blueprint in ShopBluePrints)
        {
            blueprint.PreviewElement(value, element, quality);
        }
        foreach (var blueprint in OwnBluePrints)
        {
            blueprint.PreviewElement(value, element, quality);
        }

    }
}
