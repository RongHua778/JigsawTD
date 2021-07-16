using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class BluePrintShopUI : IUserInterface
{
    bool Showing = false;//���ƶ���
    Animator anim;
    public GameObject ShopBtnObj;

    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;
    [SerializeField] Text NextRefreshTurnsTxt = default;
    [SerializeField] Transform shopContent = default;
    [SerializeField] Text PerfectElementTxt = default;

    public List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();//�̵��䷽��
    public List<BluePrintGrid> OwnBluePrints = new List<BluePrintGrid>();//ӵ���䷽��

    int shopCapacity = 3;
    public int ShopCapacity { get => shopCapacity; set => shopCapacity = value; }
    int nextRefreshTrun = 0;
    public int NextRefreshTrun //�´��Զ�ˢ�»غ�
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
            NextRefreshTurnsTxt.text = nextRefreshTrun + "�غϺ�ˢ��";
        }
    }



    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        anim = this.GetComponent<Animator>();
        NextRefreshTrun = 4;
    }

    public void SetPerfectElementCount(int count)
    {
        PerfectElementTxt.text = "ӵ������Ԫ�أ�" + count;
    }

    public void RefreshShop(int level, int cost)//ˢ���̵�
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

    public void AddBluePrint(Blueprint bluePrint, bool isShopBluePrint)//������ͼ��isShopblueprint�жϼ����̵껹��ӵ��
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

    public void ShopBtnClick()//�����̵����򿪶���
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

    public void MoveBluePrintToPocket(BluePrintGrid grid)//���̵��䷽����ӵ��
    {
        grid.InShop = false;
        grid.transform.SetAsLastSibling();
        OwnBluePrints.Add(grid);
        if (ShopBluePrints.Contains(grid))
            ShopBluePrints.Remove(grid);
        grid.transform.SetAsLastSibling();
    }

    public void RefreshBtnClick()
    {
        GameManager.Instance.RefreshShop(30);
    }



    public void CompositeBluePrint(BluePrintGrid grid)//�ϳɶ�Ӧ���䷽
    {
        grid.BluePrint.BuildBluePrint();
        ConstructHelper.GetCompositeTurretByBluePrint(grid.BluePrint);
        RemoveGrid(grid);
        CheckAllBluePrint();
        GameManager.Instance.HideTips();
        //���ý�����Ϣ
        GameEndUI.TotalComposite++;
    }

    private void RemoveGrid(BluePrintGrid grid)//�Ƴ���Ӧ���䷽���������б�
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

    public void CheckAllBluePrint()//��������䷽�Ƿ��ɺϳ�����
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

    public void GetARandomBluePrintToPocket(int level)//����ֵ����������һ���䷽
    {
        Blueprint bluePrint = ConstructHelper.GetRandomBluePrintByLevel(level);
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
