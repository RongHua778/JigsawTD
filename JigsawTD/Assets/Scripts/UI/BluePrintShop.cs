using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class BluePrintShop : MonoBehaviour
{
    bool Showing = false;//控制动画
    Animator anim;

    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;
    [SerializeField] Text NextRefreshTurnsTxt = default;
    [SerializeField] Transform shopContent = default;

    public List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();//商店配方表
    public List<BluePrintGrid> OwnBluePrints = new List<BluePrintGrid>();//拥有配方表

    int nextRefreshTrun = 0;
    public int NextRefreshTrun //下次自动刷新回合
    { get => nextRefreshTrun; 
        set 
        {
            nextRefreshTrun = value;
            if (nextRefreshTrun <= 0)
            {
                nextRefreshTrun = 3;
                RefreshShop(0);
            }
            NextRefreshTurnsTxt.text = nextRefreshTrun + "回合后刷新";
        } 
    }

    private void Start()
    {
        NextRefreshTrun = 4;
        
        GameEvents.Instance.onCheckBluePrint += CheckAllBluePrint;
        GameEvents.Instance.onLuckyFull += GetARandomBluePrintToPocket;

        anim = this.GetComponent<Animator>();
        RefreshShop(0);
    }

    private void OnDisable()
    {
        GameEvents.Instance.onCheckBluePrint -= CheckAllBluePrint;
        GameEvents.Instance.onLuckyFull -= GetARandomBluePrintToPocket;
    }
    public void RefreshShop(int cost)//刷新商店
    {
        if (!LevelUIManager.Instance.ConsumeMoney(cost))
            return;
        foreach (var grid in ShopBluePrints.ToList())
        {
            RemoveGrid(grid);
        }
        ShopBluePrints.Clear();
        for (int i = 0; i < 3; i++)
        {
            TurretAttribute compositeTurret = GameManager.Instance.GetRandomCompositeAttributeByLevel();
            Blueprint bluePrint = GameManager.Instance.GetSingleBluePrint(compositeTurret);
            AddBluePrint(bluePrint,true);
        }
    }

    public void AddBluePrint(Blueprint bluePrint,bool isShopBluePrint)//增加蓝图，isShopblueprint判断加入商店还是拥有
    {
        GameObject bluePrintObj = ObjectPool.Instance.Spawn(bluePrintGridPrefab.gameObject);
        bluePrintObj.transform.SetParent(shopContent);
        BluePrintGrid grid = bluePrintObj.GetComponent<BluePrintGrid>();
        grid.SetElements(this, shopContent.GetComponent<ToggleGroup>(), bluePrint);
        if (isShopBluePrint)
        {
            bluePrintObj.transform.SetAsFirstSibling();
            ShopBluePrints.Add(grid);
        }
        else
        {
            bluePrintObj.transform.SetAsLastSibling();
            OwnBluePrints.Add(grid);
        }
    }

    public void ShopBtnClick()//播放商店界面打开动画
    {
        Showing = !Showing;
        anim.SetBool("Showing", Showing);
    }

    public void MoveBluePrintToPocket(BluePrintGrid grid)//把商店配方移入拥有
    {
        OwnBluePrints.Add(grid);
        ShopBluePrints.Remove(grid);
        grid.transform.SetAsLastSibling();
    }

    public void CompositeBluePrint(BluePrintGrid grid)//合成对应的配方
    {
        grid.BluePrint.BuildBluePrint();
        GameManager.Instance.GenerateCompositeShape(grid.BluePrint);
        RemoveGrid(grid);
        CheckAllBluePrint();

        //设置结算信息
        GameEndPanel.TotalComposite++;
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
        ObjectPool.Instance.UnSpawn(grid.gameObject);
    }

    private void CheckAllBluePrint()//检查所有配方是否达成合成条件
    {
        foreach(var bluePrint in ShopBluePrints)
        {
            bluePrint.CheckElements();
        }
        foreach (var bluePrint in OwnBluePrints)
        {
            bluePrint.CheckElements();
        }
    }

    private void GetARandomBluePrintToPocket()//幸运值满，随机获得一个配方
    {
        TurretAttribute compositeTurret = GameManager.Instance.GetRandomCompositeAttributeByLevel();
        Blueprint bluePrint = GameManager.Instance.GetSingleBluePrint(compositeTurret);
        AddBluePrint(bluePrint,false);
    }

}
