using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class BluePrintShop : MonoBehaviour
{
    bool Showing = false;
    Animator anim;
    int nextRefreshTrun = 0;
    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;
    [SerializeField] Text NextRefreshTurnsTxt = default;
    [SerializeField] Transform shopContent = default;

    public List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();
    public List<BluePrintGrid> OwnBluePrints = new List<BluePrintGrid>();

    public int NextRefreshTrun { get => nextRefreshTrun; 
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
        anim = this.GetComponent<Animator>();
        RefreshShop(0);
    }
    public void RefreshShop(int cost)
    {
        if (!LevelUIManager.Instance.ConsumeMoney(cost))
            return;
        foreach (var grid in ShopBluePrints.ToList())
        {
            grid.RemoveBuildPrint();
        }
        ShopBluePrints.Clear();
        for (int i = 0; i < 3; i++)
        {
            TurretAttribute compositeTurret = StaticData.Instance.GetRandomCompositionTurret();
            Blueprint bluePrint = GameManager.Instance.GetSingleBluePrint(compositeTurret);
            AddBluePrint(bluePrint);
        }
    }

    public void AddBluePrint(Blueprint bluePrint)
    {
        GameObject bluePrintObj = ObjectPool.Instance.Spawn(bluePrintGridPrefab.gameObject);
        bluePrintObj.transform.SetParent(shopContent);
        bluePrintObj.transform.SetAsFirstSibling();
        bluePrintObj.GetComponent<BluePrintGrid>().SetElements(this, shopContent.GetComponent<ToggleGroup>(), bluePrint);
        ShopBluePrints.Add(bluePrintObj.GetComponent<BluePrintGrid>());
    }

    public void ShopBtnClick()
    {
        Showing = !Showing;
        anim.SetBool("Showing", Showing);
    }

    public void MoveBluePrintToPocket(BluePrintGrid grid)
    {
        OwnBluePrints.Add(grid);
        ShopBluePrints.Remove(grid);
        grid.transform.SetAsLastSibling();
    }

    private void CheckAllBluePrint()
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

}
